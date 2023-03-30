using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject scoreTeamText;
    [SerializeField] private GameObject timeText;
    
    [SerializeField] private Transform blueFlagRespawn;
    [SerializeField] private Transform redFlagRespawn;

    private Transform spawnBlueFlagTransform;
    private Transform spawnRedFlagTransform;

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
}
