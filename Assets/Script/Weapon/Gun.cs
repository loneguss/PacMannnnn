using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Gun : NetworkBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public string playerName;
    public string playerRealName;


    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    [SerializeField] private bool isGunActive = false;
    public bool IsGunActive { get => isGunActive; set => isGunActive = value; }
    
    [SerializeField] private SpriteRenderer gunSprite;
    public SpriteRenderer GunSprite { get => gunSprite; set => gunSprite = value; }

    private void Start()
    {
        if (!IsOwner) return;
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
        SpawnBulletServerRpc(playerName,playerRealName);
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
    void SpawnBulletServerRpc(string _playerName,string _playerRealName,  ServerRpcParams serverRpcParams = default)
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.ownerName = _playerName;
        bullet.realName = _playerRealName;
        var clientId = serverRpcParams.Receive.SenderClientId;
        bullet.GetComponent<NetworkObject>().Spawn(true);
        shootBullet(bullet, firePoint.up * bulletSpeed * Time.deltaTime);
    }

    
    void shootBullet(Bullet bullet,Vector2 power)
    {
        bullet.GetComponent<Rigidbody2D>().velocity = power;
    }


    [ClientRpc]
    public void RemoveGunClientRpc()
    {
        Debug.Log("removegun ");
        if (isGunActive)
        {
            Debug.Log("Remove Gun");
            gunSprite.enabled = false;
            isGunActive = false;
        }
        else if (!isGunActive)
        {
            Debug.Log("Add Gun");
            gunSprite.enabled = true;
            isGunActive = true;
        }
    }
    
    [ServerRpc]
    public void RemoveGunServerRpc()
    {
       RemoveGunClientRpc();
    }
    
}