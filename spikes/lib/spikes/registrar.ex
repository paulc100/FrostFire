defmodule Spikes.Registrar do
  @moduledoc """
  Manages client information within the context of the 'Spike'
  """
  require Logger
  use GenServer, restart: :temporary
  @crlf "\r\n"

  @doc """
  Starts the registrar with the given options
  """
  def start_link(opts) do
    GenServer.start_link(__MODULE__, :ok, opts)
  end

  @doc """
  Initiates the registration process for a client that newly connects over a socket
  """
  def register(server, socket), do: GenServer.cast(server, {:register, socket})

  @doc """
  Retrieves the clients in the registrar
  """
  def clients(server), do: GenServer.call(server, :clients)

  @doc """
  Retrieve a client name from its associated pid
  """
  def client_name(server, pid), do: GenServer.call(server, {:client_name, pid})

  @doc """
  Retrieve a client pid from its associated name in-game
  """
  def client_pid(server, name), do: GenServer.call(server, {:client_pid, name})

  @impl true
  def init(:ok) do
    {:ok, {%{}, %{}}}
  end

  @impl true
  def handle_call(:clients, _from, {users, _} = state) do
    {:reply, users, state}
  end

  @impl true
  def handle_call({:client_name, pid}, _from, {_, pids} = state) do
    %{^pid => name} = pids
    {:reply, name, state}
  end

  @impl true
  def handle_call({:client_pid, name}, _from, {names, _} = state) do
    %{^name => pid} = names
    {:reply, pid, state}
  end

  @impl true
  def handle_cast({:register, socket}, state) do
    case tcp_welcome_msg(socket) do
      :ok ->
        {:noreply, state}

      {:error, :closed} ->
        Logger.error("Client connection closed for socket: #{inspect(socket)}}")
        {:noreply, state}

      err ->
        Logger.error(
          "Oops! There was an error registering the client: #{inspect(socket)}, #{err}"
        )

        {:noreply, state}
    end
  end

  @impl true
  def handle_info({:tcp, socket, data}, {names, pids} = _state) do
    {:ok, pid} =
      DynamicSupervisor.start_child(
        Spikes.ClientServerSupervisor,
        {Spikes.ClientServer, {socket, []}}
      )

    release(socket, pid)

    [name | _] = String.split(data)

    Logger.debug(
      "Received TCP socket information, name: #{name}, pid: #{inspect(pid)}, socket: #{inspect(socket)}"
    )

    broadcast_join(name)

    {:noreply, {Map.put(names, name, pid), Map.put(pids, pid, name)}}
  end

  @impl true
  def handle_info({:tcp_closed, _socket}, state) do
    {:noreply, state}
  end

  @impl true
  def handle_info({:DOWN, _, :process, pid, _reason}, {names, pids}) do
    %{^pid => name} = pids

    Spikes.Chat.broadcast_event(Spikes.Chat, "<" <> name <> " has left the game" <> ">" <> @crlf)
    {:noreply, {Map.delete(names, name), Map.delete(pids, pid)}}
  end

  defp tcp_welcome_msg(socket) do
    :gen_tcp.send(socket, "Welcome to FrostFire! Please enter a nickname." <> @crlf)
  end

  defp broadcast_join(name) do
    Spikes.Chat.broadcast_event(
      Spikes.Chat,
      "<" <> name <> " has joined the game" <> ">" <> @crlf
    )
  end

  defp release(socket, pid) do
    Process.monitor(pid)
    :gen_tcp.controlling_process(socket, pid)
  end
end
