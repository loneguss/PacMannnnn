using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float spinSpeed = 100f;
    public float moveSpeed = 5f;
    private bool isSpinning = true;
    
    //gun
    private Gun _gun;

    public override void OnNetworkSpawn()
    {
        _gun = GetComponent<Gun>();
    }

    

    void Update()
    {
        if (!IsOwner) return;
        
        
        if (Input.GetKey(KeyCode.Space))
        {
            isSpinning = false;
        }
        else
        {
            isSpinning = true;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _gun.Shoot();
        }
            
        if (isSpinning)
        {
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }
}