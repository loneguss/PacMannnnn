using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void LateUpdate()
    {
        Vector3 cam = new Vector3(0, 0, -10);
        if(target == null) return;
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);
    }
}
