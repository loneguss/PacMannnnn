using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WinLose : NetworkBehaviour
{
    private TimeCountdown _timeCountdown;
    private PointCounter _pointCounter;

    [Header("UI Elements")] 
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Image panelImg;
    [SerializeField] private TextMeshProUGUI teamWinText;
    
    private void Start()
    {
        _timeCountdown = GetComponent<TimeCountdown>();
        _pointCounter = GetComponent<PointCounter>();
    }
    
    [ServerRpc]
    public void WinnerPanelServerRpc()
    {
        if (_timeCountdown.CheckTimeOut())
        {
            if (_pointCounter.Team1Score > _pointCounter.Team2Score) 
                WinnerPanelClientRpc(Color.red, "Red Team Wins !!!");
            
            else if (_pointCounter.Team1Score < _pointCounter.Team2Score) 
                WinnerPanelClientRpc(Color.blue, "Blue Team Wins !!!");
            
            else WinnerPanelClientRpc(Color.black, "Draw !!!");
            
        }

    }

    [ClientRpc]
    void WinnerPanelClientRpc(Color color, string text)
    {
        teamWinText.text = text;
        teamWinText.color = color;
        gameEndPanel.SetActive(true);
    }
}
