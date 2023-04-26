using Unity.Netcode;
using UnityEngine;

public class FlagDrop : MonoBehaviour
{
    private Player _player;
    private GrabFlag _grabFlag;
    
    [Header("Flag Prefabs")]
    [SerializeField] private GameObject flagBlue;
    [SerializeField] private GameObject flagRed;
    
    private void Start()
    {
        _player = GetComponent<Player>();
        _grabFlag = GetComponent<GrabFlag>();
    }
    
    [ServerRpc]
    public void DropFlagServerRpc()
    {
        _grabFlag.DropFlagClientRpc();
        // _grabFlag.IsGrab = false;
        // _grabFlag.FlagSprite.enabled = false;

        if (_player.GetPlayerTeam() == Player.Team.Blue)
        {
            Debug.Log("Red Flag Dropped");
            Instantiate(flagRed, _player.PlayerPos.position, Quaternion.identity).GetComponent<NetworkObject>().Spawn(true);
        }
        else
        {
            Debug.Log("Blue Flag Dropped");
            Instantiate(flagBlue, _player.PlayerPos.position, Quaternion.identity).GetComponent<NetworkObject>().Spawn(true);
        }
        
    }
}
