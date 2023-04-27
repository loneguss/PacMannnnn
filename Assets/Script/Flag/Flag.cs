using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Player.Team team;

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player"))
         {
             if(col.GetComponent<Player>().GetPlayerTeam() != team && !col.GetComponent<Player>().DeadDelay())
             {
                Debug.Log("Flag has taken");
                DestroyFlagServerRpc();
                FindObjectOfType<NetworkFeed>().Feed(col.GetComponent<Player>().GetPlayerRealName(), NetworkFeed.FeedType.stoleFlag," ");
             }
             // else if (col.GetComponent<Player>().GetPlayerTeam() == team && !col.GetComponent<Player>().DeadDelay())
             // {
             //     Debug.Log("Get flag back");
             //     DestroyFlagServerRpc();
             //     if (col.GetComponent<Player>().GetPlayerTeam() == Player.Team.Red && team == Player.Team.Red)
             //     {
             //         FindObjectOfType<GameManager>().RedFlagSpawnServerRpc();
             //     }
             //     else FindObjectOfType<GameManager>().BlueFlagSpawnServerRpc();
             // }
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
