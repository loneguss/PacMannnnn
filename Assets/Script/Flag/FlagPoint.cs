using System;
using Unity.Netcode;
using UnityEngine;

public class FlagPoint : NetworkBehaviour
{
    [SerializeField] private Player.Team flagPointTeam;
    
    public Player.Team FlagPointTeam
    {
        get => flagPointTeam;
        set => flagPointTeam = value;
    }
}
