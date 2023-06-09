using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkFeed : NetworkBehaviour
{
    public enum FeedType
    {
        Kill,
        stoleFlag,
        finishFlag,
        DropFlag,
    }

    [SerializeField] private Sprite[] feedSprite;
    [SerializeField] private TextMeshProUGUI  UI_playerNameA;
    [SerializeField] private TextMeshProUGUI  UI_playerNameB;
    [SerializeField] private Image UI_feedIcon;

    public void Feed(string playerNameA,FeedType _feedType,string playerNameB)
    {
        FeedServerRpc(playerNameA,_feedType,playerNameB);
    }

    [ServerRpc (RequireOwnership = false)]
    public void FeedServerRpc(string playerNameA,FeedType _feedType,string playerNameB)
    {
        FeedClientRpc(playerNameA,_feedType,playerNameB);
    }
    
    [ClientRpc]
    private void FeedClientRpc(string playerNameA,FeedType _feedType,string playerNameB)
    {
        UI_playerNameA.text = playerNameA;
        UI_feedIcon.sprite = GetSprite(_feedType);
        UI_playerNameB.text = playerNameB;
        GetComponent<Animation>().Play("Feed_anim");

    }
    
    private Sprite GetSprite(FeedType _feedType)
    {
        if (_feedType == FeedType.Kill)
        {
            return feedSprite[0];
        }
        if (_feedType == FeedType.stoleFlag)
        {
            return feedSprite[1];
        }
        if (_feedType == FeedType.finishFlag)
        {
            return feedSprite[2];
        }
    
        
        return feedSprite[0];
    }
}
