using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject playerBase;
    private bool isDead = false;
    private PlayerTeleport _playerTeleport;
    [SerializeField] private NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>
        ("player",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    
    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner) return;
        playerName.Value = "Player:" + NetworkManager.Singleton.LocalClientId.ToString();
        _playerTeleport = GetComponent<PlayerTeleport>();
        playerBase = GameObject.FindWithTag("Base");
        GetComponent<Gun>().playerName = playerName.Value.ToString();
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

    public string GetPlayerName()
    {
        return playerName.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc()
    {
        Debug.Log("fuickkk");
    }
}
