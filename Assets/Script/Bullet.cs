using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public string ownerName;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        SetOwnerServerRpc();
        StartCoroutine(DeleteBullet(2));
    }

    // Update is called once per frame
    void Update()
    {
        
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
            player.Dead();
           FindObjectOfType<NetworkFeed>().FeedServerRpc(ownerName,NetworkFeed.FeedType.Kill,player.GetPlayerName());
           DeleteBulletServerRpc();
        }
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
