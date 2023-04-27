using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class DisplayName : NetworkBehaviour
{
    private void FixedUpdate()
    {
        
        if(!IsOwner) return;
        
        string myName = SetPlayerName.Instance.GetName();
        setNameServerRpc(myName);
        HideName();

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
    }

    void HideName()
    {
        GetComponent<TextMeshPro>().text = "";
        
    }
}
