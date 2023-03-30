using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimeCountdown : NetworkBehaviour
{
    private GameManager _gameManager;
    [Header("Time")]
    [SerializeField] private float timeLimit = 600f;
    private float _timeLeftServer;
    private float _timeLeftClient;
    
    [SerializeField] private TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _timeLeftServer = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient)
        {
            DisplayTimeServerRpc();
        }
    }

    public void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(_timeLeftServer / 60);
        float seconds = Mathf.FloorToInt(_timeLeftServer % 60);
        if (_timeLeftServer > 0)
        {
            Debug.Log(NetworkManager.NetworkTimeSystem.LocalTime);
            _timeLeftServer -= Time.deltaTime;
            minutes -= Time.deltaTime;
            seconds -= Time.deltaTime;
            
            Debug.Log(timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds) + "Time JAaaaaa");
        }
        if(_timeLeftServer <= 0) timeText.text = "Time's Up!";
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DisplayTimeServerRpc()
    {
        DisplayTimeClientRpc();
    }
    
    [ClientRpc]
    public void DisplayTimeClientRpc()
    {
        _timeLeftClient = _timeLeftServer;
        float minutes = Mathf.FloorToInt(_timeLeftClient / 60);
        float seconds = Mathf.FloorToInt(_timeLeftClient % 60);
        if (_timeLeftServer > 0 && _timeLeftClient > 0)
        {
            _timeLeftServer -= Time.deltaTime;
            minutes -= Time.deltaTime;
            seconds -= Time.deltaTime;
            
            Debug.Log(timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds) + "Time JAaaaaa");
        }
        if(_timeLeftServer <= 0 && _timeLeftClient <= 0) timeText.text = "Time's Up!";
    }
}
