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
        
        StartCoroutine(RefreshName(0.2f));
        
    }

    IEnumerator RefreshName(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            string myName = SetPlayerName.Instance.GetName();
            setNameServerRpc(myName);
            Debug.Log(myName);
        }
       
    }

    private void Update()
    {
        if(!IsOwner) return;

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
        GetComponent<TextMeshPro>().enabled = false;

    }
}
