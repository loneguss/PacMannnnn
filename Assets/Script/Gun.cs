using Unity.Netcode;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    

    public void Shoot()
    {
        if (!IsOwner) return;
        if (Time.time < nextFireTime) return;

        SpawnBulletServerRpc();
        

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.up);
        if (hitInfo)
        {
            Debug.Log("Hit " + hitInfo.transform.name);
            // do something with hitInfo
        }
        
        nextFireTime = Time.time + 1f / fireRate;
    }

    
    [ServerRpc ] void SpawnBulletServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed * Time.deltaTime;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        
    }
}