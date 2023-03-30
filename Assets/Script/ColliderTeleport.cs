using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;
using Unity.Netcode.Components;
using Unity.Netcode;


public class ColliderTeleport : MonoBehaviour
{
    public GameObject des;

    public bool Preserve_XAxis;
    public bool Preserve_YAxis;
    public bool Preserve_ZAxis;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        {
            return;
        }

        var playerMover = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerMover == null || playerMover.IsTeleporting)
        {
            return;
        }

        var networkTranform = collision.gameObject.GetComponent<ClientNetworkTransform>();
        if (networkTranform == null || des == null)
        {
            return;
        }

        var objectTranform = networkTranform.transform;
        var position = des.transform.position;

        if (Preserve_XAxis)
        {
            position.x = objectTranform.position.x;
        }
        
        if (Preserve_YAxis)
        {
            position.y = objectTranform.position.y;
        }
        if (Preserve_ZAxis)
        {
            position.z = objectTranform.position.z;
        }
        playerMover.Teleporting(position);
    }
    
}