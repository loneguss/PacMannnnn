using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject scoreTeamText;
    [SerializeField] private GameObject timeText;

    public GameObject TimeText
    {
        get => timeText;
        set => timeText = value;
    }
    
    [Header("Flag Respawn")]
    [SerializeField] private Transform blueFlagRespawn;
    [SerializeField] private Transform redFlagRespawn;
    
    private Transform spawnBlueFlagTransform;
    private Transform spawnRedFlagTransform;
    
    [Header("Base")]
    [SerializeField] private GameObject redBase, blueBase;

    private bool isGame = false;

    private Player _player;

    [SerializeField] private GameObject[] test;
    

    private Transform spawnFlagTransform;

    public string playerName;
        
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            DisplayUIServerRpc();
        }
    }

    #region -Flag Respawn-
    
    public IEnumerator RedFlagSpawn()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Red Flag Spawned");
        if (!IsOwner) yield break;
        spawnRedFlagTransform = Instantiate(redFlagRespawn);
        spawnRedFlagTransform.GetComponent<NetworkObject>().Spawn(true);
    }
    
    public IEnumerator BlueFlagSpawn()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Blue Flag Spawned");
        if (!IsOwner) yield break;
        spawnBlueFlagTransform = Instantiate(blueFlagRespawn);
        spawnBlueFlagTransform.GetComponent<NetworkObject>().Spawn(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RedFlagSpawnServerRpc()
    {
        StartCoroutine(RedFlagSpawn());
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void BlueFlagSpawnServerRpc()
    {
        StartCoroutine(BlueFlagSpawn());
    }

    #endregion
    

    #region -Display UI-

    [ClientRpc]
    public void DisplayUIClientRpc()
    {
        scoreTeamText.SetActive(true);
        timeText.SetActive(true);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DisplayUIServerRpc()
    {
        DisplayUIClientRpc();
    }

    #endregion


    #region -Start Game-

    [ServerRpc]
    public void StartGameServerRpc()
    {
        StartGameClientRpc();
    }
    
    [ClientRpc]
    private void StartGameClientRpc()
    {
        StartGame();
    }
    
    public void StartGame()
    {
        GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
        test = allPlayer;
        foreach (var i in allPlayer)
        {
            var currentPlayer = i.GetComponent<Player>();
            if (currentPlayer.GetPlayerTeam() == Player.Team.Red)
            {
                currentPlayer.SetPlayerBase(redBase);
                currentPlayer.Spawn(Player.Team.Red); }
            else if (currentPlayer.GetPlayerTeam() == Player.Team.Blue)
            {
                currentPlayer.SetPlayerBase(blueBase);
                currentPlayer.Spawn(Player.Team.Blue);
            }
            Debug.Log(currentPlayer.GetPlayerName());
            
        }
        
        isGame = true;
    }

    #endregion
    

    public bool GetIsGame()
    {
        return isGame;
    }
    
}
