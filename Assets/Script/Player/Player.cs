using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Mathematics;
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
    [SerializeField] private Transform playerPos;

    public Transform PlayerPos
    {
        get => playerPos;
        set => playerPos = value;
    }

    private bool isDead = false;

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }

    private GrabFlag grabFlag;

    [SerializeField] private PlayerTeleport _playerTeleport;

    [SerializeField] private string PlayerName;
    [SerializeField] private string PlayerRealName;


    [SerializeField] private Team _team = Team.White;

    [SerializeField] private GameObject playerName;

    public GameObject playerNameObject;

    // Start is called before the first frame update
    void Start()
    {


        if (IsOwner)
        {
            PlayerRealName = SetPlayerName.Instance.GetName();

            FindObjectOfType<CameraFollow>().target = this.transform;
            SpawnNameServerRpc();
            SetClientNameServerRpc();

        }

        if (IsServer)
        {
            FindObjectOfType<Lobby>().ChangeMaxPlayerServerRpc();
            SetTeamServerRpc(_team);
        }

        playerBase = GameObject.FindWithTag("Base");
        _playerTeleport = GetComponent<PlayerTeleport>();
        //GameObject.FindObjectOfType<Lobby>().ChangeMaxPlayerServerRpc();


        //PlayerName = "Player:" + NetworkManager.Singleton.LocalClientId.ToString();


        playerPos = this.transform;

        grabFlag = GetComponent<GrabFlag>();
        GetComponent<Gun>().playerRealName = PlayerRealName;

    }

    [ServerRpc]
    public void SetClientNameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var PlayerName = "Player:" + clientId;
        SetClientNameClientRpc(PlayerName);
    }

    [ClientRpc]
    public void SetClientNameClientRpc(string _name)
    {
        this.PlayerName = _name;
        GetComponent<Gun>().playerName = PlayerName;

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!IsOwner) return;
        Bullet bullet = col.gameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            if (bullet.ownerName == PlayerName)
            {
                Debug.Log("same name");
                return;
            }
            
            Dead();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            FindObjectOfType<NetworkFeed>().Feed(bullet.realName, NetworkFeed.FeedType.Kill, PlayerRealName);
            bullet.DeleteBulletNow();

        }
    }


        public bool CheckDead()
        {
            if (isDead) return true;

            return false;
        }
        
        IEnumerator DeadDelayCoroutine()
        {
            yield return new WaitForSeconds(2f);
            grabFlag.IsGrab = false;
            grabFlag.FlagSprite.enabled = false;
            isDead = false;
        }

        public void Dead()
        {
            grabFlag.DropFlagServerRpc();

            isDead = true;
            Debug.Log("U Dead" + PlayerName);
            if (isDead)
            {
                StartCoroutine(DeadDelayCoroutine());
            }

            
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _playerTeleport.Teleport(playerBase.gameObject);

            Vector3 des = new Vector3();
            des = playerBase.transform.position;

            GetComponent<PlayerMovement>().Teleporting(des);
        }

        public void Spawn(Team t)
        {
            _playerTeleport.Teleport(playerBase.gameObject);

            Vector3 des = new Vector3();
            des = playerBase.transform.position;
            GetComponent<PlayerMovement>().Teleporting(des);
            if (IsServer)
            {
                StartCoroutine(SpawnTimer(t));
            }

        }

        IEnumerator SpawnTimer(Team t)
        {
            yield return new WaitForSeconds(0.3f);
            SetTeamServerRpc(t);
        }

        public string GetPlayerName()
        {
            return PlayerName;
        }

        public string GetPlayerRealName()
        {
            return PlayerRealName;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnNameServerRpc(ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;

            var floatingName = Instantiate(playerName, this.transform);
            floatingName.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            floatingName.AddComponent<CameraFollow>();
            floatingName.GetComponent<CameraFollow>().target = transform;
            floatingName.GetComponent<CameraFollow>().offset = new Vector3(0, 1, 0);
            floatingName.GetComponent<CameraFollow>().smoothTime = 0.01f;

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

        public PlayerTeleport GetPlayerTeleport()
        {
            return _playerTeleport;
        }

        public GameObject GetPlayerBase()
        {
            return playerBase.gameObject;
        }



        public void SetPlayerBase(GameObject _base)
        {
            playerBase = _base;
        }
    }
