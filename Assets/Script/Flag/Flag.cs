using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Player.Team team;

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player"))
         {
             if(col.GetComponent<Player>().GetPlayerTeam() != team)
             {
                Debug.Log("Flag has taken");
                DestroyFlagServerRpc();
             }
         }
     }
    
    [ServerRpc(RequireOwnership = false)]
    public void DestroyFlagServerRpc()
    {
        Debug.Log("Destroy flag in Server ?");
        GetComponent<NetworkObject>().Despawn(true);
        DestroyFlagClientRpc();
    }

    [ClientRpc]
    void DestroyFlagClientRpc()
    {
        Debug.Log("Destroy flag in Client ?");
        Destroy(this.gameObject);
    }
}
