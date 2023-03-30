using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    private PointCounter _pointCounter;
    private GameManager _gameManager;

    private void Start()
    {
        _pointCounter = FindObjectOfType<PointCounter>().GetComponent<PointCounter>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player"))
         {
             
             if (IsClient)
             {
                 Debug.Log("Player has entered the flag zone");
                 _pointCounter.FlagPointServerRpc(1); 
                 DestroyFlagServerRpc();
             }
         }

         if (col.CompareTag("Player"))
         {
             
             // if (OwnerClientId == 1)
             // {
             //     Debug.Log("Player has entered the flag zone");
             //     _pointCounter.FlagPointClientRpc(0, 1); 
             //     DestroyFlagServerRpc();
             // }
             // {
             //     Debug.Log("banana zone");
             //     _pointCounter.FlagPointClientRpc(1, 1);
             //     DestroyFlagServerRpc();
             // }
             // _pointCounter.FlagPointServerRpc(1);
         }
     }
    
    [ServerRpc(RequireOwnership = false)]
    void DestroyFlagServerRpc()
    {
        Debug.Log("Destroy Flag");
        GetComponent<NetworkObject>().Despawn(true);
    }
}
