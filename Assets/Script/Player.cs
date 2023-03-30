using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public enum Team
    {
        Red,
        Blue,
        White
    }


    [SerializeField] private GameObject playerBase;
    private bool isDead = false;
    private PlayerTeleport _playerTeleport;

    [SerializeField] private NetworkVariable<FixedString64Bytes> playerName = new NetworkVariable<FixedString64Bytes>
        ("player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private Team _team = Team.White;

    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            FindObjectOfType<Lobby>().ChangeMaxPlayerServerRpc();
        }


        if (!IsOwner) return;
        SetTeamServerRpc(_team);
        //GameObject.FindObjectOfType<Lobby>().ChangeMaxPlayerServerRpc();
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


    [ServerRpc(RequireOwnership = false)]
    public void SetTeamServerRpc(Team _team)
    {

        this._team = _team;

        if (_team == Team.Red)
        {
            SetTeamClientRpc(Color.red, _team);
        }

        if (_team == Team.White)
        {
            SetTeamClientRpc(Color.white, _team);
        }

        if (_team == Team.Blue)
        {
            SetTeamClientRpc(Color.blue, _team);
        }
    }

    [ClientRpc]
    private void SetTeamClientRpc(Color color, Team _team)
    {
        this._team = _team;
        GetComponent<SpriteRenderer>().color = color;
    }

    public Team GetPlayerTeam()
    {
        return _team;
    }
}
