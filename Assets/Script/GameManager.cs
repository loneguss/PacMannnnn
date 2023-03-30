using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject scoreTeamText;

    [SerializeField] private Transform blueFlagRespawn;
    [SerializeField] private Transform redFlagRespawn;

    private Transform spawnBlueFlagTransform;
    private Transform spawnRedFlagTransform;

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
        // if (Input.GetKeyDown(KeyCode.S) && IsServer)
        // {
        //     spawnBlueFlagTransform = Instantiate(blueFlagRespawn);
        //     spawnBlueFlagTransform.GetComponent<NetworkObject>().Spawn(true);
        //     
        //     spawnRedFlagTransform = Instantiate(redFlagRespawn);
        //     spawnRedFlagTransform.GetComponent<NetworkObject>().Spawn(true);
        // }
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
