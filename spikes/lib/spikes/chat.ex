defmodule Spikes.Chat do
  @moduledoc """
  Chat service for clients connecting to a 'Spike'
  """
  use GenServer

  @doc """
  Starts the ChatServer that is distributed between clients
  """
  def start_link(opts) do
    GenServer.start_link(__MODULE__, :ok, opts)
  end

  @doc """
  Broadcasts a client message across the server
  """
  def broadcast(server, from, message) do
    name = Spikes.Registrar.client_name(Spikes.Registrar, from)
    GenServer.call(server, {:broadcast, {name, message}})
  end

  @doc """
  Broadcasts a server event (message) to all clients connected to the server
  """
  def broadcast_event(server, event) do
    GenServer.cast(server, {:broadcast_event, event})
  end

  @doc """
  Returns the pids associated with a tuple of clients
  """
  def client_pids() do
    Spikes.Registrar.clients(Spikes.Registrar) |> Enum.map(fn {_, pid} -> pid end)
  end

  @impl true
  def init(:ok) do
    {:ok, []}
  end

  @impl true
  def handle_call({:broadcast, {name, message}}, _from, state) do
    formatted_message = "<" <> name <> "> " <> message
    client_pids() |> Enum.each(&Spikes.ClientServer.push(&1, formatted_message))

    {:reply, :ok, [formatted_message | Enum.take(state, 9)]}
  end

  @impl true
  def handle_cast({:broadcast_event, event}, state) do
    client_pids() |> Enum.each(&Spikes.ClientServer.push(&1, event))

    {:noreply, [event | Enum.take(state, 9)]}
  end
end
