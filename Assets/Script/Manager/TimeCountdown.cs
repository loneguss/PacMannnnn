using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimeCountdown : NetworkBehaviour
{
    private GameManager _gameManager;
    private WinLose _winLose;
    [Header("Time")]
    [SerializeField] private float timeLimit = 600f;

    [Header("Time Server")]
    private float _timeLeftServer;
    public float TimeLeftServer
    {
        get => _timeLeftServer;
        set => _timeLeftServer = value;
    }
    
    [Header("Time Client")]
    private float _timeLeftClient;
    public float TimeLeftClient
    {
        get => _timeLeftClient;
        set => _timeLeftClient = value;
    }
    
    [Header("Time Text")]
    [SerializeField] private TextMeshProUGUI timeText;

    void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _winLose = GetComponent<WinLose>();
        _timeLeftServer = timeLimit;
    }
    
    void Update()
    {
        if (_gameManager.GetIsGame() == false)
        {
            return;
        }
        if (IsServer)
        {
            DisplayTimeServerRpc();
        }
    }

    #region Time UI

        [ServerRpc(RequireOwnership = false)]
        public void DisplayTimeServerRpc()
        {
            _timeLeftClient = _timeLeftServer;
            float minutes = Mathf.FloorToInt(_timeLeftClient / 60);
            float seconds = Mathf.FloorToInt(_timeLeftClient % 60);
            if (_timeLeftServer > 0 && _timeLeftClient > 0)
            {
                _timeLeftServer -= Time.deltaTime;
                minutes -= Time.deltaTime;
                seconds -= Time.deltaTime;
                
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            if (_timeLeftServer <= 0 && _timeLeftClient <= 0)
            {
                DisplayTimeClientRpc(" ");
                _winLose.WinnerPanelServerRpc();
            }
            DisplayTimeClientRpc(timeText.text);
        }
        
        [ClientRpc]
        public void DisplayTimeClientRpc(string text)
        {
            timeText.text = text;
        }

    #endregion
    

    public bool CheckTimeOut()
    {
        if (_timeLeftServer <= 0 && _timeLeftClient <= 0)
        {
            return true;
        }

        return false;
    }
    
}
