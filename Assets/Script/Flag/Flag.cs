using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Player.Team team;

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player"))
         {
             if(col.GetComponent<Player>().GetPlayerTeam() != team && !col.GetComponent<Player>().CheckDead())
             {
                Debug.Log("Flag has taken");
                DestroyFlagServerRpc();
                FindObjectOfType<NetworkFeed>().Feed(col.GetComponent<Player>().GetPlayerRealName(), NetworkFeed.FeedType.stoleFlag," ");
             }
         }
     }
    
    [ServerRpc(RequireOwnership = false)]
    void DestroyFlagServerRpc()
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
