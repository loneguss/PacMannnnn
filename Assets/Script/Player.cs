using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerBase;
    private bool isDead = false;
    private PlayerTeleport _playerTeleport;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerTeleport = GetComponent<PlayerTeleport>();
        playerBase = GameObject.FindWithTag("Base");
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
}
