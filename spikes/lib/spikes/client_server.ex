defmodule Spikes.ClientServer do
  @moduledoc """
  Surfaces server data and broadcasts to the client
  """
  require Logger
  use GenServer, restart: :temporary

  @doc """
  Starts the ClientServer on the given socket with the provided options
  """
  def start_link({socket, opts}) do
    GenServer.start_link(__MODULE__, socket, opts)
  end

  @doc """
  Pushes a message ('packet') to a client
  """
  def push(server, packet), do: GenServer.cast(server, {:push, packet})

  @impl true
  def init(socket) do
    Logger.debug("#{__MODULE__} started. PID: #{inspect(self())}, socket: #{inspect(socket)}")
    {:ok, socket}
  end

  @impl true
  def handle_cast({:push, packet}, socket) do
    case :gen_tcp.send(socket, packet) do
      :ok -> {:noreply, socket}
      {:error, :closed} -> {:stop, :normal, socket}
    end
  end

  @impl true
  def handle_info({:tcp, _socket, msg}, socket) do
    :ok = Spikes.Chat.broadcast(Spikes.Chat, self(), msg)
    {:noreply, socket}
  end

  @impl true
  def handle_info({:tcp_closed, _socket}, socket) do
    {:stop, :normal, socket}
  end
end
