using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPlayerController : NetworkBehaviour 
{
    [SerializeField] private Vector3 movement = new Vector3();

    [Client]
    private void Update() 
    {
        if (!hasAuthority) { return; }

        if (!Input.GetKeyDown(KeyCode.Space)) { return; }

        MoveClient();
    }

    [Command]
    private void MoveClient() {
        RpcMove();
    }

    [ClientRpc]
    private void RpcMove() => transform.Translate(movement);
}
