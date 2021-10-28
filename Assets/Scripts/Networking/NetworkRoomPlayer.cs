using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayer : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject lobbyUI = null;
    [SerializeField]
    private Text[] playerNameTexts = new Text[4]; 
    [SerializeField]
    private Text[] playerReadyTexts = new Text[4];
    [SerializeField]
    private Button startGameButton = null;

    // Variables that can only be changed on the server
    // Once changed, these values are sent to all clients
    // Kind of like a server event
    [SyncVar(hook = nameof(OnDisplayNameChanged))]
    public string PlayerName = "Loading...";
    [SyncVar(hook = nameof(OnReadyStatusChanged))]
    public bool IsReady = false;

    private bool isHost;
    public bool IsHost
    {
        set
        {
            isHost = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby lobby;
    private NetworkManagerLobby Lobby
    {
        get
        {
            if (lobby != null)
                return lobby;

            return lobby = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(PlayerNameInput.PlayerName);

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Lobby.RoomPlayers.Add(this);

        UpdateLobbyDisplay();
    }

    public override void OnStopClient()
    {
        Lobby.RoomPlayers.Remove(this);

        UpdateLobbyDisplay();
    }

    public void OnReadyStatusChanged(bool oldValue, bool newValue) => UpdateLobbyDisplay();
    public void OnDisplayNameChanged(string oldValue, string newValue) => UpdateLobbyDisplay();

    private void UpdateLobbyDisplay()
    {
        if (!hasAuthority)
        {
            // If networked player triggered update, find local player UI
            foreach (var player in Lobby.RoomPlayers) 
            {
                if (player.hasAuthority)
                {
                    player.UpdateLobbyDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting for player...";
            playerNameTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Lobby.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Lobby.RoomPlayers[i].PlayerName;
            playerReadyTexts[i].text = Lobby.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStartGame(bool isReadyToStartGame)
    {
        if (!isHost)
            return;

        startGameButton.interactable = isReadyToStartGame;
    }

    // Commands are used to update data on the server
    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        PlayerName = playerName;        
    }

    [Command]
    public void CmdSetReadyUp()
    {
        IsReady = !IsReady;

        Lobby.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Lobby.RoomPlayers[0].connectionToClient != connectionToClient)
            return;
    }
}
