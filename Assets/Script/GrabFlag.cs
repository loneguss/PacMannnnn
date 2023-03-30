using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GrabFlag : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer flagSprite;
    private Player _player;
    private PointCounter _pointCounter;

    private NetworkVariable<bool> isGrab = new NetworkVariable<bool>();

    public NetworkVariable<bool> IsGrab
    {
        get => isGrab;
        set => isGrab = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        _pointCounter = FindObjectOfType<PointCounter>().GetComponent<PointCounter>();
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Flag"))
        {
            GrabFlagServerRpc();
        }
        
        if(col.CompareTag("FlagPoint") && isGrab.Value)
        {
            if (_player.GetPlayerTeam() == Player.Team.Red)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(1, 0);
                flagSprite.enabled = false;
            }

            if (_player.GetPlayerTeam() == Player.Team.Blue)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(0, 1); 
                flagSprite.enabled = false;
            }
        }
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void GrabFlagServerRpc()
    {
        if (!flagSprite.enabled && isGrab.Value)
        {
            isGrab.Value = false;
            return;
        }
        if (!isGrab.Value)
        {
            GrabFlagClientRpc();
            flagSprite.enabled = true;
            isGrab.Value = true;
        }
        else if (isGrab.Value)
        {
            GrabFlagClientRpc();
            flagSprite.enabled = false;
            isGrab.Value = false;
        }
    }
    
    [ClientRpc]
    public void GrabFlagClientRpc()
    {
        if (!isGrab.Value)
        {
            Debug.Log("Player has grabbed the flag");
            if (_player.GetPlayerTeam() == Player.Team.Red)
            {
                flagSprite.color = Color.blue;
            }
            if (_player.GetPlayerTeam() == Player.Team.Blue)
            {
                flagSprite.color = Color.red;
            }
        }
    }
}
