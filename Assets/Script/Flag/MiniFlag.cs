using System;
using Unity.Netcode;
using UnityEngine;

public class MiniFlag : NetworkBehaviour
{
    private Flag flag;

    private void Start()
    {
        flag = GetComponent<Flag>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Get flag back");
            DestroyMiniFlagServerRpc();
            if (flag.team == Player.Team.Blue && col.GetComponent<Player>().GetPlayerTeam() == Player.Team.Blue)
            {
                FindObjectOfType<GameManager>().BlueFlagSpawnServerRpc();
            }
            else FindObjectOfType<GameManager>().RedFlagSpawnServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyMiniFlagServerRpc()
    {
        Debug.Log("Destroy mini flag server rpc");
        GetComponent<NetworkObject>().Despawn(true);
        DestroyMiniFlagClientRpc();
    }
    
    [ClientRpc]
    void DestroyMiniFlagClientRpc()
    {
        Debug.Log("Destroy mini flag client rpc");
        Destroy(this.gameObject);
    }
}
