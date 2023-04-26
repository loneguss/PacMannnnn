using Unity.Netcode;
using UnityEngine;

public class FlagDrop : MonoBehaviour
{
    private Player _player;
    
    [Header("Flag Prefabs")]
    [SerializeField] private GameObject flagBlue;
    [SerializeField] private GameObject flagRed;
    
    private void Start()
    {
        _player = GetComponent<Player>();
    }
    
    [ServerRpc]
    public void DropFlagServerRpc()
    {
        if (_player.GetPlayerTeam() == Player.Team.Blue)
        {
            Debug.Log("Red Flag Dropped");
            Instantiate(flagBlue, _player.PlayerPos.position, Quaternion.identity).GetComponent<NetworkObject>().Spawn(true);
        }
        else
        {
            Debug.Log("Blue Flag Dropped");
            Instantiate(flagRed, _player.PlayerPos.position, Quaternion.identity).GetComponent<NetworkObject>().Spawn(true);
        }
        
        // if (_player.IsDead)
        // {
        //     if (_player.GetPlayerTeam() == Player.Team.Blue)
        //     {
        //         Debug.Log("Red Flag Dropped");
        //         Instantiate(flagBlue, _player.PlayerPos.position, Quaternion.identity).GetComponent<NetworkObject>().Spawn(true);
        //     }
        //     else
        //     {
        //         Debug.Log("Blue Flag Dropped");
        //         Instantiate(flagRed, _player.PlayerPos.position, Quaternion.identity).GetComponent<NetworkObject>().Spawn(true);
        //     }
        // }
    }
}
