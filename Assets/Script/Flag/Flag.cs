using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Player.Team team;

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player"))
         {
             if(col.GetComponent<Player>().GetPlayerTeam() == Player.Team.Blue && team == Player.Team.Red)
             {
                Debug.Log("Blue team take a flag");
                DestroyFlagServerRpc();
             }
             else if(col.GetComponent<Player>().GetPlayerTeam() == Player.Team.Red && team == Player.Team.Blue)
             { 
                 Debug.Log("Red team take a flag");
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
