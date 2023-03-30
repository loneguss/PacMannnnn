using System;
using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{
    public Transform flagTransform;
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
             
             if (IsServer)
             {
                 Debug.Log("Player has entered the flag zone");
                 _pointCounter.FlagPointServerRpc(1); 
                 DestroyFlagServerRpc();
             }
             // _pointCounter.FlagPointServerRpc(1);
             Destroy(this.gameObject);
         }

         if (col.CompareTag("Player"))
         {
             
             if (!IsServer)
             {
                 Debug.Log("banana zone");
                 _pointCounter.FlagPointClientRpc(1, 1);
                 DestroyFlagServerRpc();
             }
             // _pointCounter.FlagPointServerRpc(1);
             Destroy(this);

         }
     }
    
    [ServerRpc(RequireOwnership = false)]
    void DestroyFlagServerRpc()
    {
        Debug.Log("Destroy Flag");
        GetComponent<NetworkObject>().Despawn(true);
    }
}
