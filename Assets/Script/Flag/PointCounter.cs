using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PointCounter : NetworkBehaviour
{
    [Header("Red Team 1")]
    [SerializeField] private int team1Score;
    public int Team1Score
    {
        get => team1Score;
        set
        {
            team1Score = value;
            team1ScoreText.text = team1Score.ToString();
        }
    }
    
    [Header("Blue Team 2")]
    [SerializeField] private int team2Score;
    public int Team2Score
    {
        get => team2Score;
        set
        {
            team2Score = value;
            team2ScoreText.text = team2Score.ToString();
        }
    }
    
    [Header("Team Score Text")]
    [SerializeField] private TextMeshProUGUI team1ScoreText; // Red Team
    [SerializeField] private TextMeshProUGUI team2ScoreText; // Blue Team

    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        
        team1ScoreText.text = "Red Team: " + team1Score.ToString();
        team2ScoreText.text = "Blue Team: " + team2Score.ToString();
        
    }
    
   
    
    [ServerRpc(RequireOwnership = false)]
    public void FlagPointServerRpc(int valueTeam1, int valueTeam2)
    {
        team1Score += valueTeam1;
        team2Score += valueTeam2;
        FlagPointClientRpc(team1Score,team2Score);
    }
    
    [ClientRpc]
    void FlagPointClientRpc(int _team1Score, int _team2score)
    {
        team1Score = _team1Score;
        team2Score = _team2score;

        team1ScoreText.text =  team1Score.ToString();
        team2ScoreText.text =  team2Score.ToString();

        _gameManager.DisplayUIServerRpc();
        _gameManager.DisplayUIClientRpc();
    }
}
