using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    private PointCounter _pointCounter;
    private GameManager _gameManager;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        _pointCounter = FindObjectOfType<PointCounter>().GetComponent<PointCounter>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player") && _player.GetPlayerTeam() == Player.Team.Red)
         {
             Debug.Log("Player has entered the flag zone");
             _pointCounter.FlagPointServerRpc(1, 0); 
             DestroyFlagServerRpc();
         }
         if (col.CompareTag("Player") && _player.GetPlayerTeam() == Player.Team.Blue)
         {
             Debug.Log("Player has entered the flag zone");
             _pointCounter.FlagPointServerRpc(0, 1); 
             DestroyFlagServerRpc();
         }
     }
    
    [ServerRpc(RequireOwnership = false)]
    void DestroyFlagServerRpc()
    {
        Debug.Log("Destroy Flag");
        GetComponent<NetworkObject>().Despawn(true);
    }
}
