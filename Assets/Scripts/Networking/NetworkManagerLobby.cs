using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager 
{
    private const string networkedPrefabsFolder = "NetworkedPrefabs";

    [SerializeField]
    private int minPlayers = 2;

    [Scene] 
    [SerializeField]
    private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField]
    private NetworkRoomPlayer roomPlayerPrefab = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>(networkedPrefabsFolder).ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>(networkedPrefabsFolder);

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene) 
        {
            bool isHost = RoomPlayers.Count == 0;

            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsHost = isHost;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStartGame(IsReadyToStart());
        }
    }

    public bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
            return false;

        foreach(var player in RoomPlayers)
        {
            if (!player.IsReady)
                return false;
        }

        return true;
    }
}
