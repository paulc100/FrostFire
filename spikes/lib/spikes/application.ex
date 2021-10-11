defmodule Spikes.Application do
  # See https://hexdocs.pm/elixir/Application.html
  # for more information on OTP Applications
  @moduledoc false

  use Application

  @impl true
  def start(_type, _args) do
    port = String.to_integer(System.get_env("PORT") || "4040")

    children = [
      {Spikes.Chat, name: Spikes.Chat},
      {DynamicSupervisor, strategy: :one_for_one, name: Spikes.ClientServerSupervisor},
      {Spikes.Registrar, name: Spikes.Registrar},
      {Spikes.Dispatcher, port}
    ]

    # See https://hexdocs.pm/elixir/Supervisor.html
    # for other strategies and supported options
    opts = [strategy: :one_for_one, name: Spikes.Supervisor]
    Supervisor.start_link(children, opts)
  end
end
