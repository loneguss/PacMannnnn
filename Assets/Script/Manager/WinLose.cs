using System;
using Unity.Netcode;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    private TimeCountdown _timeCountdown;
    private PointCounter _pointCounter;

    private void Start()
    {
        _timeCountdown = GetComponent<TimeCountdown>();
        _pointCounter = GetComponent<PointCounter>();
    }

    private void Update()
    {
        CheckWinnerServerRpc();
    }

    [ServerRpc]
    public void CheckWinnerServerRpc()
    {
        if (_timeCountdown.CheckTimeOut())
        {
            Debug.Log("Game End");
            if (_pointCounter.Team1Score > _pointCounter.Team2Score)
            {
                Debug.Log("Red Team Wins !!!");
            }
            else if (_pointCounter.Team1Score < _pointCounter.Team2Score)
            {
                Debug.Log("Blue Team Wins !!!");
            }
            else
            {
                Debug.Log("Draw !!!");
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
    public void CheckWinnerClientRpc()
    {
        
    }
}
