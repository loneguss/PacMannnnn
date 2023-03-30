using Unity.Netcode;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    [SerializeField] private bool isGunActive = false;
    public bool IsGunActive { get => isGunActive; set => isGunActive = value; }
    [SerializeField] private SpriteRenderer gun;

    private void Start()
    {
        if (!IsOwner) return;
        RemoveGunServerRpc();
    }

    void Update()
    {
        // if(!IsOwner) return;
        // RemoveGunServerRpc();
        /*     
         if (Input.GetButtonDown("Fire1") &s& Time.time >= nextFireTime)
         {
             Shoot();
             nextFireTime = Time.time + 1f / fireRate;
         }
         */
    }

    public void Shoot()
    {
        if (!IsOwner) return;
        if (Time.time < nextFireTime) return;

        if (!isGunActive) return;
        
        Debug.Log("Shoot");
        SpawnBulletServerRpc();
        nextFireTime = Time.time + 1f / fireRate;

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.up);
        if (hitInfo)
        {
            Debug.Log("Hit " + hitInfo.transform.name);
            // do something with hitInfo
        }
        
        // nextFireTime = Time.time + 1f / fireRate;
    }

    
    [ServerRpc] 
    void SpawnBulletServerRpc()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<NetworkObject>().Spawn(true);
        rb.velocity = firePoint.up * bulletSpeed * Time.deltaTime;
    }
    
    
    [ClientRpc]
    public void RemoveGunClientRpc()
    {
        
        Debug.Log("removegun ");
        if (isGunActive)
        {
            Debug.Log("Remove Gun");
            gun.enabled = false;
            isGunActive = false;
        }
        else if (!isGunActive)
        {
            Debug.Log("Add Gun");
            gun.enabled = true;
            isGunActive = true;
        }
    }
    
    [ServerRpc]
    public void RemoveGunServerRpc()
    {
       RemoveGunClientRpc();
    }
    
}