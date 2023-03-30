using Unity.Netcode;
using UnityEngine;

public class Flag : NetworkBehaviour
{

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.CompareTag("Player"))
         {
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
