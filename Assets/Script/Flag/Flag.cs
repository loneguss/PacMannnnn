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
                team = Player.Team.Red;
                DestroyFlagServerRpc();
             }
             else if(col.GetComponent<Player>().GetPlayerTeam() == Player.Team.Red && team == Player.Team.Blue)
             {
                 Debug.Log("Red team take a flag");
                 team = Player.Team.Blue;
                 DestroyFlagServerRpc();
             }
         }
     }
    
    [ServerRpc(RequireOwnership = false)]
    void DestroyFlagServerRpc()
    {
        Debug.Log("Destroy Flag");
        GetComponent<NetworkObject>().Despawn(true);
    }
}
