
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using UnityEngine;
using Unity.Netcode;


public class PlayerTeleport : MonoBehaviour
{
    public bool Preserve_XAxis;
    public bool Preserve_YAxis;
    public bool Preserve_ZAxis;


    public void Teleport(GameObject des)
    {
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        {
            return;
        }

        var playerMover = this.gameObject.GetComponent<PlayerMovement>();
        if (playerMover == null || playerMover.IsTeleporting)
        {
            return;
        }

        var networkTranform = this.gameObject.GetComponent<ClientNetworkTransform>();
        if (networkTranform == null || des == null)
        {
            return;
        }

        var objectTranform = networkTranform.transform;
        var position = des.transform.position;

        if (Preserve_XAxis)
        {
            position.x = objectTranform.position.x;
        }
        
        if (Preserve_YAxis)
        {
            position.y = objectTranform.position.y;
        }
        if (Preserve_ZAxis)
        {
            position.z = objectTranform.position.z;
        }
        playerMover.Teleporting(position);
    }
    
}
