using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GrabFlag : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer flagSprite;
    private Player _player;
    private PointCounter _pointCounter;
    private Flag _flag;
    private FlagPoint _flagPoint;
    private GameManager _gameManager;

    private bool isGrab;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        _pointCounter = FindObjectOfType<PointCounter>().GetComponent<PointCounter>();
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Flag"))
        {
            if (col.gameObject.GetComponent<Flag>().team == Player.Team.Blue && _player.GetPlayerTeam() == Player.Team.Red)
            {
                GrabFlagServerRpc();
            }
            else if (col.gameObject.GetComponent<Flag>().team == Player.Team.Red && _player.GetPlayerTeam() == Player.Team.Blue)
            {
                GrabFlagServerRpc();
            }
        }

        if(col.CompareTag("FlagPoint") && isGrab)
        {
            if (_player.GetPlayerTeam() == Player.Team.Red && col.GetComponent<FlagPoint>().FlagPointTeam == Player.Team.Red)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(1, 0);
                StartCoroutine(_gameManager.BlueFlagSpawn());
                flagSprite.enabled = false;
            }

            if (_player.GetPlayerTeam() == Player.Team.Blue && col.GetComponent<FlagPoint>().FlagPointTeam == Player.Team.Blue)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(0, 1);
                StartCoroutine(_gameManager.RedFlagSpawn());
                flagSprite.enabled = false;
            }
        }
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void GrabFlagServerRpc()
    {
        if (!flagSprite.enabled && isGrab)
        {
            isGrab = false;
            return;
        }
        if (!isGrab)
        {
            if (_player.GetPlayerTeam() == Player.Team.Red)
            {
                GrabFlagClientRpc(Color.blue);
            }
            if (_player.GetPlayerTeam() == Player.Team.Blue)
            {
                GrabFlagClientRpc(Color.red);
            }
            
            flagSprite.enabled = true;
            isGrab = true;
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
            flagSprite.color = _color;
            flagSprite.enabled = true;
        }

    }
    
    [ClientRpc]
    public void DropFlagClientRpc()
    {
        isGrab = false;
        flagSprite.enabled = false;
    }
}
