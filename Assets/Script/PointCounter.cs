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
        
        team1ScoreText.text = team1Score.ToString();
        team2ScoreText.text = team2Score.ToString();
        
    }
    
   
    
    [ServerRpc(RequireOwnership = false)]
    public void FlagPointServerRpc(int value)
    {
        team1Score += value;
        team2Score += value;
        FlagPointClientRpc(team1Score,team1Score);
    }
    
    [ClientRpc]
    public void FlagPointClientRpc(int _team1Score, int _team2score)
    {
        team1Score = _team1Score;
        team2Score = _team2score;

        team1ScoreText.text = team1Score.ToString();
        team1ScoreText.text = team2Score.ToString();

        
        _gameManager.DisplayScoreServerRpc();
        _gameManager.DisplayScoreClientRpc();
    }
}
