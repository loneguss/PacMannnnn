using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject scoreTeamText;

    [SerializeField] private Transform greenFlagRespawn;
    public Transform GreenFlagRespawn
    {
        get => greenFlagRespawn;
        set => greenFlagRespawn = value;
    }

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

        if (Input.GetKeyDown(KeyCode.S))
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
}
