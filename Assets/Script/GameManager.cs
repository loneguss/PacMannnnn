using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject scoreTeamText;

    [SerializeField] private Transform greenFlagRespawn;

    [SerializeField] private GameObject redBase, blueBase;
    public Transform GreenFlagRespawn
    {
        get => greenFlagRespawn;
        set => greenFlagRespawn = value;
    }

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
            scoreTeamText.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.S) && IsServer)
        {
            spawnFlagTransform = Instantiate(greenFlagRespawn);
            spawnFlagTransform.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    [ClientRpc]
    public void DisplayScoreClientRpc()
    {
        scoreTeamText.SetActive(true);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DisplayScoreServerRpc()
    {
        scoreTeamText.SetActive(true);
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
                currentPlayer.Dead();
               
            }
            else if (currentPlayer.GetPlayerTeam() == Player.Team.Blue)
            {
                currentPlayer.SetPlayerBase(blueBase);
                currentPlayer.Dead();
            }
            Debug.Log(currentPlayer.GetPlayerName());
            
        }
    }
}
