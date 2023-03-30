using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Lobby : NetworkBehaviour
{
    private int MaxPlayer;
    [SerializeField] private TMP_InputField MaxPlayerTextHolder;


    [SerializeField] private int redTeamPLayer;
    [SerializeField] private int blueTeamPLayer;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private Button _startButton;

    [SerializeField] private TextMeshPro lobbyInfoText;

    private void Start()
    {
        lobbyInfoText.text =
            "Max Player: " + MaxPlayer + " | Blue Team: " + blueTeamPLayer + " | Red Team: " + blueTeamPLayer + " |";
    }

    private void Update()
    {
        if(!IsServer)
        {
            _startButton.gameObject.SetActive(false);
            lobbyPanel.SetActive(false);
            return;
        }
        
        lobbyPanel.SetActive(true);

        
        if (redTeamPLayer > 0 && blueTeamPLayer > 0)
        {
            if (redTeamPLayer == blueTeamPLayer)
            {
                _startButton.gameObject.SetActive(true);

            }
            else
            {
                _startButton.gameObject.SetActive(false);

            }
        }
        else
        {
            _startButton.gameObject.SetActive(false);

        }
    }

    [ServerRpc]
    public void AddPlayerInTeamServerRpc(Player.Team team)
    {
        switch (team)
        {
            case Player.Team.Red:
                redTeamPLayer++;
                break;
            case Player.Team.Blue:
                blueTeamPLayer++;
                break;
        }
        
        SetPlayerValueClientRpc(redTeamPLayer, blueTeamPLayer,MaxPlayer);

    }
    
    [ServerRpc]
    public void DecrasePlayerInTeamServerRpc(Player.Team team)
    {
        switch (team)
        {
            case Player.Team.Red:
                redTeamPLayer--;
                break;
            case Player.Team.Blue:
                blueTeamPLayer--;
                break;
        }

        SetPlayerValueClientRpc(redTeamPLayer, blueTeamPLayer,MaxPlayer);

    }

    [ServerRpc]
    public void ChangeMaxPlayerServerRpc()
    {
        MaxPlayer = int.Parse(MaxPlayerTextHolder.text);
        SetPlayerValueClientRpc(redTeamPLayer, blueTeamPLayer,MaxPlayer);

    }

    [ClientRpc]
    private void SetPlayerValueClientRpc(int r,int b,int maxPlayer)
    {
        redTeamPLayer = r;
        blueTeamPLayer = b;
        MaxPlayer = maxPlayer;
        lobbyInfoText.text =
            "Max Player: " + MaxPlayer + " | Blue Team: " + blueTeamPLayer + " | Red Team: " + redTeamPLayer + " |";
    }

}
