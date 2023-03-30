using System;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerMovement :  NetworkBehaviour
{
    public float spinSpeed = 100f;
    public float moveSpeed = 5f;
    private bool isSpinning = true;
    private bool n_IsTeleporting;
    private Rigidbody2D m_Rigidbody;

    
    //teleport shit
    private float n_TickFrequency;
    private float n_DelayInputForTeleport;
    private Quaternion n_PreviousRotation;
    private RigidbodyInterpolation2D n_OriginalRigibodyInterpolation;
    
    public bool IsTeleporting
    {
        get
        {
            return n_IsTeleporting;
        }
    }
    
    
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    
    //gun
    private Gun _gun;

    public override void OnNetworkSpawn()
    {
        _gun = GetComponent<Gun>();
    }

    private void LateUpdate()
    {
        if (!IsSpawned || !IsOwner )
        {
            return;
        }

        if (Time.realtimeSinceStartup >= n_DelayInputForTeleport)
        {
            n_IsTeleporting = false;
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.interpolation = n_OriginalRigibodyInterpolation;
        }
    }
    
    public void Teleporting(Vector3 des)
    {
        if (IsSpawned && IsOwner && !n_IsTeleporting)
        {
            n_IsTeleporting = true;
            m_Rigidbody.isKinematic = true;
            n_OriginalRigibodyInterpolation = m_Rigidbody.interpolation;
            m_Rigidbody.interpolation = RigidbodyInterpolation2D.None;

            n_DelayInputForTeleport = Time.realtimeSinceStartup + (3f * n_TickFrequency);

            transform.rotation = n_PreviousRotation;
            
            GetComponent<ClientNetworkTransform>().Teleport(des,transform.rotation,transform.localScale);
        }
        
        
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