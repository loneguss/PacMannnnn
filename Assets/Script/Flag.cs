using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Player.Team team;
    private Player _player;
    private void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
    }

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
    void DestroyFlagServerRpc()
    {
        Debug.Log("Destroy Flag");
        GetComponent<NetworkObject>().Despawn(true);
    }
}
