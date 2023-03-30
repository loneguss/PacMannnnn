using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamPickingArea : NetworkBehaviour
{
    public Player.Team Team;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsServer) return;
        if (!col.transform.CompareTag("Player")) return;
        
        col.gameObject.GetComponent<Player>().SetTeamServerRpc(Team);
        FindObjectOfType<Lobby>().AddPlayerInTeamServerRpc(Team);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!IsServer) return;

        if (!col.transform.CompareTag("Player")) return;
        
        col.gameObject.GetComponent<Player>().SetTeamServerRpc(Player.Team.White);
        FindObjectOfType<Lobby>().DecrasePlayerInTeamServerRpc(Team);
    }
}