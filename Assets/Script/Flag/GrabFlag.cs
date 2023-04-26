using System;
using Unity.Netcode;
using UnityEngine;

public class GrabFlag : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer flagSprite;
    private Player _player;
    private PointCounter _pointCounter;
    private Flag _flag;
    private FlagPoint _flagPoint;
    private GameManager _gameManager;

    [SerializeField] private bool isGrab = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _flag = FindObjectOfType<Flag>().GetComponent<Flag>();
    }

    void Start()
    {
        _player = GetComponent<Player>();
        _pointCounter = FindObjectOfType<PointCounter>().GetComponent<PointCounter>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        isGrab = false;
        flagSprite.enabled = false;
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Flag") && !isGrab && col.GetComponent<Flag>().team == _player.GetPlayerTeam()) return;
        if (col.CompareTag("Flag") && !isGrab && col.GetComponent<Flag>().team != _player.GetPlayerTeam())
        {
            Debug.Log( col.GetComponent<Flag>().team + " Flag "  + _player.GetPlayerTeam() + " Can Grab");
            GrabFlagServerRpc();
        }
        
        
        // if (col.gameObject.CompareTag("Flag") && !isGrab)
        // {
        //     if (col.gameObject.GetComponent<Flag>().team == Player.Team.Blue)
        //     {
        //         Debug.Log("For Red Team");
        //         if (_player.GetPlayerTeam() == Player.Team.Red)
        //         {
        //             Debug.Log("red team" + " Flag color : " + col.gameObject.GetComponent<Flag>().team + " Player color : " + _player.GetPlayerTeam());
        //             GrabFlagServerRpc();
        //         }
        //         else return;
        //     }
        //     else if (col.gameObject.GetComponent<Flag>().team == Player.Team.Red)
        //     {
        //         Debug.Log("For Blue Team");
        //         if (_player.GetPlayerTeam() == Player.Team.Blue)
        //         {
        //             Debug.Log("blue team" + " Flag color : " + col.gameObject.GetComponent<Flag>().team + " Player color : " + _player.GetPlayerTeam());
        //             GrabFlagServerRpc();
        //         }
        //         else return;
        //     }
        // }
        
        if(col.CompareTag("FlagPoint") && isGrab)
        {
            if (_player.GetPlayerTeam() == Player.Team.Red && col.gameObject.GetComponent<FlagPoint>().FlagPointTeam == Player.Team.Red)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(1, 0);
                // _gameManager.BlueFlagSpawnServerRpc();
                _gameManager.BlueFlagSpawnClientRpc();
                flagSprite.enabled = false;
            }

            if (_player.GetPlayerTeam() == Player.Team.Blue && col.gameObject.GetComponent<FlagPoint>().FlagPointTeam == Player.Team.Blue)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(0, 1);
                // _gameManager.RedFlagSpawnServerRpc();
                _gameManager.RedFlagSpawnClientRpc();
                flagSprite.enabled = false;
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void GrabFlagServerRpc()
    {
        if (!flagSprite.enabled && !isGrab)
        {
            if (_player.GetPlayerTeam() == Player.Team.Red && _flag.team != _player.GetPlayerTeam())
            {
                Debug.Log("Blue Flag Grabbed");
                GrabFlagClientRpc(Color.blue);
            }
            if (_player.GetPlayerTeam() == Player.Team.Blue  && _flag.team != _player.GetPlayerTeam())
            {
                Debug.Log("Red Flag Grabbed");
                GrabFlagClientRpc(Color.red);
            }
        }
        else
        {
            DropFlagClientRpc();
        }
    }
    
    [ClientRpc]
    public void GrabFlagClientRpc(Color _color)
    {
        if (!isGrab)
        {
            Debug.Log("Flag Grabbed");
            flagSprite.color = _color;
            flagSprite.enabled = true;
            isGrab = true;
        }
    }
    
    // [ServerRpc]
    // public void DropFlagServerRpc()
    // {
    //     DropFlagClientRpc();
    // }
    
    [ClientRpc]
    public void DropFlagClientRpc()
    {
        Debug.Log("Drop Flag " + _flag.team);
        isGrab = false;
        flagSprite.enabled = false;
    }
}
