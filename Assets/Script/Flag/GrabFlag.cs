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
        if (col.gameObject.CompareTag("Flag") && !isGrab)
        {
            if (col.gameObject.GetComponent<Flag>().team == Player.Team.Blue && _player.GetPlayerTeam() == Player.Team.Red)
            {
                Debug.Log("Blue flag");
                GrabFlagServerRpc();
            }
            
            if (col.gameObject.GetComponent<Flag>().team == Player.Team.Red && _player.GetPlayerTeam() == Player.Team.Blue)
            {
                Debug.Log("Red flag");
                GrabFlagServerRpc();
            }
        }

        if(col.CompareTag("FlagPoint") && isGrab)
        {
            if (_player.GetPlayerTeam() == Player.Team.Red && col.gameObject.GetComponent<FlagPoint>().FlagPointTeam == Player.Team.Red)
            {
                GrabFlagServerRpc();
                _pointCounter.FlagPointServerRpc(1, 0);
                StartCoroutine(_gameManager.BlueFlagSpawn());
                flagSprite.enabled = false;
            }

            if (_player.GetPlayerTeam() == Player.Team.Blue && col.gameObject.GetComponent<FlagPoint>().FlagPointTeam == Player.Team.Blue)
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
                Debug.Log("Blue Flag Grabbed");
                GrabFlagClientRpc(Color.blue);
            }
            if (_player.GetPlayerTeam() == Player.Team.Blue)
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
    
    [ClientRpc]
    public void DropFlagClientRpc()
    {
        isGrab = false;
        flagSprite.enabled = false;
    }
}
