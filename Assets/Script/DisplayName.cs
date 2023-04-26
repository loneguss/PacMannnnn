using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class DisplayName : NetworkBehaviour
{
    private void Start()
    {
        if(!IsOwner) return;
        string myName = SetPlayerName.Instance.GetName();
        setNameServerRpc(myName);
    }


    [ServerRpc]
    void setNameServerRpc(string _name)
    {
        setNameClientRpc(_name);
    }
    
    [ClientRpc]
    void setNameClientRpc(string _name)
    {
        GetComponent<TextMeshPro>().text = _name;
        Debug.Log("dewdw");
    }
}
