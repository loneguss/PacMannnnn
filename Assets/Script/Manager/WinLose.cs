using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class WinLose : MonoBehaviour
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

        // teamWinText = GetComponent<TextMeshProUGUI>();
        // panelImg = GetComponent<Image>();
    }

    private void Update()
    {
        // WinnerPanelServerRpc();
    }

    [ServerRpc]
    public void WinnerPanelServerRpc()
    {
        if (_timeCountdown.CheckTimeOut())
        {
            Debug.Log("Game End");
            if (_pointCounter.Team1Score > _pointCounter.Team2Score)
            {
                // Debug.Log("Red Team Wins !!!");
                WinnerPanelClientRpc();
                teamWinText.color = Color.red;
                teamWinText.text = "Red Team Wins !!!";
                // panelImg.color = Color.red;
            }
            else if (_pointCounter.Team1Score < _pointCounter.Team2Score)
            {
                // Debug.Log("Blue Team Wins !!!");
                WinnerPanelClientRpc();
                teamWinText.color = Color.blue;
                teamWinText.text = "Blue Team Wins !!!";
                // panelImg.color = Color.blue;
            }
            else
            {
                //Debug.Log("Draw !!!");
                WinnerPanelClientRpc();
                teamWinText.text = "Draw !!!";
            }
        }
        // if (_timeCountdown.TimeoutCheck)
        // {
        //     Debug.Log("Game End");
        //     if (_pointCounter.Team1Score > _pointCounter.Team2Score)
        //     {
        //         Debug.Log("Red Team Wins !!!");
        //     }
        //     else if (_pointCounter.Team1Score < _pointCounter.Team2Score)
        //     {
        //         Debug.Log("Blue Team Wins !!!");
        //     }
        //     else
        //     {
        //         Debug.Log("Draw !!!");
        //     }
        // }
        // else
        // {
        //     Debug.Log("Game is still going");
        // }
        
    }

    [ClientRpc]
    public void WinnerPanelClientRpc()
    {
        gameEndPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
