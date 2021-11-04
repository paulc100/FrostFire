using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour 
{
    [SerializeField]
    private GameObject playerPrefab = null;

    private static List<Transform> spawnPoints = new List<Transform>();

    private int nextSpawnIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer() => NetworkManagerFrostFire.OnServerReadied += SpawnPlayer;

    [ServerCallback]
    private void OnDestroy() => NetworkManagerFrostFire.OnServerReadied -= SpawnPlayer;    

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextSpawnIndex);

        if (spawnPoint == null)
        {
            Debug.LogError("Missing spawn point for player " + nextSpawnIndex);
            return;
        }

        // 'Instantiate' in a NetworkBehaviour will instantiate the prefab on the network
        // NetworkServer.Spawn spawns the instantiated game object on the rest of the clients
        // Passing a connection to 'Spawn' gives the gameObject authority to that client, pass no connections to give authority to the server
        // ex: Spawners, see NetworkManagerFrostFire.OnServerSceneChanged
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextSpawnIndex].position, spawnPoints[nextSpawnIndex].rotation);
        NetworkServer.AddPlayerForConnection(conn, playerInstance);

        Debug.Log("Spawned player from server");

        nextSpawnIndex++;
    }
}
