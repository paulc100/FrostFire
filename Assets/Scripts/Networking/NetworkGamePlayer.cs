using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SyncVar]
    private string playerName = "Loading...";

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

    public override void OnStartClient()
    {
        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }
}
