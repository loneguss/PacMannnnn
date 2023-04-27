using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public string ownerName;
    public string realName;

    [SerializeField] private GameObject impact;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        SetOwnerServerRpc();
        StartCoroutine(DeleteBullet(2));
        
    }


    public Player targetPlayer;
    private void OnCollisionEnter2D(Collision2D col)
    { 
        if(IsServer)
        {
            Player player = col.gameObject.GetComponent<Player>();
            targetPlayer = player;
            if (targetPlayer != null)
            {
                if (player.GetPlayerName() == ownerName)
                {
                    Debug.Log("same name");
                    return;
                }
                
                SpawnImpactServerRpc(col.transform.position);
                
                //on player instead
                //player.Dead();
                //FindObjectOfType<NetworkFeed>().Feed(realName,NetworkFeed.FeedType.Kill,player.GetPlayerRealName());
           
            }
        }

    }
    

    [ServerRpc(RequireOwnership = false)]
    private void SpawnImpactServerRpc(Vector3 pos)
    {

        Vector3 newPos = new Vector3(pos.x, pos.y, -10f);
        var _impact = Instantiate(impact, pos, Quaternion.identity);
        _impact.GetComponent<NetworkObject>().Spawn(true);
    }

    public void DeleteBulletNow()
    {
        StartCoroutine(DeleteBullet(0.15f));

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
        var _realName = realName;

        SetOwnerClientRpc(_ownerName,_realName);
    }
    
    [ClientRpc]
    void SetOwnerClientRpc(string _ownerName, string _realName)
    {
        ownerName = _ownerName;
        realName = _realName;

    }
}
