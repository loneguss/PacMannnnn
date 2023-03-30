using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
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
           player.Dead();
        }
    }
    
    IEnumerator DeleteBullet(float time)
    {
      
        yield return new WaitForSeconds(time);
        DeleteBulletServerRpc();
    }

    [ServerRpc]
    void DeleteBulletServerRpc()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }
}
