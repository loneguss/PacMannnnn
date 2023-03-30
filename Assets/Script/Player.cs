using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerBase;
    private bool isDead = false;
    private PlayerTeleport _playerTeleport;
    private Flag _flag;
    private PointCounter _pointCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerTeleport = GetComponent<PlayerTeleport>();
        playerBase = GameObject.FindWithTag("Base");
        _pointCounter = FindObjectOfType<PointCounter>().GetComponent<PointCounter>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Dead()
    {
        isDead = true;
        Debug.Log("U Dead");
        _playerTeleport.Teleport(playerBase);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        // if (col.gameObject.CompareTag("Flag"))
        // {
        //     _pointCounter.FlagPoint(1);
        //     Destroy(col.gameObject);
        // }
    }
}
