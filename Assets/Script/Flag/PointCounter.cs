using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PointCounter : NetworkBehaviour
{
    [SerializeField] private int team1Score;
    [SerializeField] private int team2Score;
    
    [SerializeField] private TextMeshProUGUI team1ScoreText;
    [SerializeField] private TextMeshProUGUI team2ScoreText;

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
