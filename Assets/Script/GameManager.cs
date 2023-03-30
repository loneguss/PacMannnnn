using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject scoreTeamText;
    [SerializeField] private GameObject timeText;
    
    [SerializeField] private Transform blueFlagRespawn;
    [SerializeField] private Transform redFlagRespawn;

    [SerializeField] private GameObject redBase, blueBase;
    
    private Transform spawnBlueFlagTransform;
    private Transform spawnRedFlagTransform;

    [SerializeField] private GameObject[] test;

    private Transform spawnFlagTransform;
    public  Transform SpawnFlagTransform
    {
        get => spawnFlagTransform;
        set => spawnFlagTransform = value;
    }
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

    public IEnumerator RedFlagSpawn()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Red Flag Spawned");
        spawnRedFlagTransform = Instantiate(redFlagRespawn);
        spawnRedFlagTransform.GetComponent<NetworkObject>().Spawn(true);
    }
    
    public IEnumerator BlueFlagSpawn()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Blue Flag Spawned"); 
        spawnBlueFlagTransform = Instantiate(blueFlagRespawn);
        spawnBlueFlagTransform.GetComponent<NetworkObject>().Spawn(true);
    }
    
    

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
    }
}
