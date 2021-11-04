using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayer : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField]
    private GameObject roomUI = null;
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

    private NetworkManagerFrostFire room;
    private NetworkManagerFrostFire Room 
    {
        get
        {
            if (room != null)
                return room;

            return room = NetworkManager.singleton as NetworkManagerFrostFire;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(PlayerNameInput.PlayerName);

        roomUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateRoomDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateRoomDisplay();
    }

    public void OnReadyStatusChanged(bool oldValue, bool newValue) => UpdateRoomDisplay();
    public void OnDisplayNameChanged(string oldValue, string newValue) => UpdateRoomDisplay();

    private void UpdateRoomDisplay()
    {
        if (!hasAuthority)
        {
            // If networked player triggered update, find local player UI
            foreach (var player in Room.RoomPlayers) 
            {
                if (player.hasAuthority)
                {
                    player.UpdateRoomDisplay();
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

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].PlayerName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
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

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            return;

        Room.StartGame();
    }
}
