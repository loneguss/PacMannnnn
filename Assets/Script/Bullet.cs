using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public string ownerName;
    [SerializeField] private GameObject impact;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        SetOwnerServerRpc();
        StartCoroutine(DeleteBullet(2));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Player player = col.gameObject.GetComponent<Player>();
        
        if (player != null)
        {
            if (player.GetPlayerName() == ownerName)
            {
                Debug.Log("same name");
                return;
            }
            SpawnImpactServerRpc(col.transform.position);
            player.Dead();
           FindObjectOfType<NetworkFeed>().FeedServerRpc(ownerName,NetworkFeed.FeedType.Kill,player.GetPlayerName());
           
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnImpactServerRpc(Vector3 pos)
    {
        var _impact = Instantiate(impact, pos, Quaternion.identity);
        _impact.GetComponent<NetworkObject>().Spawn(true);
    }
    
    IEnumerator DeleteBullet(float time)
    {
      
        yield return new WaitForSeconds(time);
        DeleteBulletServerRpc();
    }

    [ServerRpc (RequireOwnership = false)]
    void DeleteBulletServerRpc()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }

    [ServerRpc]
    void SetOwnerServerRpc()
    {
        var _ownerName = ownerName;
        SetOwnerClientRpc(_ownerName);
    }
    
    [ClientRpc]
    void SetOwnerClientRpc(string _ownerName)
    {
        ownerName = _ownerName;
    }
}
