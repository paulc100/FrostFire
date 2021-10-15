defmodule Spikes.Dispatcher do
  @moduledoc """
  Dispatcher for clients connecting to a 'Spike'
  """
  require Logger
  use GenServer

  @doc """
  Begins the dispatcher to accept client connections over tcp
  """
  def start_link(port) do
    GenServer.start_link(__MODULE__, port, [])
  end

  @doc """
  Initiates the acceptance loop to handle incoming connections
  """
  def loop_acceptor(socket) do
    {:ok, client_socket} = :gen_tcp.accept(socket)
    Logger.debug("Client socket accepted: #{inspect(client_socket)}")

    register_client(client_socket)
    loop_acceptor(socket)
  end

  @doc """
  Submits a client for registration, redirects control of process to the Registrar
  """
  def register_client(socket) do
    Spikes.Registrar.register(Spikes.Registrar, socket)

    :gen_tcp.controlling_process(socket, Process.whereis(Spikes.Registrar))
  end

  @impl true
  def init(port) do
    {:ok, socket} = :gen_tcp.listen(port, [:binary, packet: :line, active: true, reuseaddr: true])
    Logger.info("Spikes (FrostFire Networking) launched on port #{port}")

    loop_acceptor(socket)
  end
end
