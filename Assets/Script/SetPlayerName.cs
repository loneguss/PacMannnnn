using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetPlayerName : MonoBehaviour
{
    public static SetPlayerName Instance;
    
    [SerializeField] private TMP_InputField name;
    private string playerName;

    public void SetName()
    {
        playerName = name.text;
    }

    public string GetName()
    {
        return (playerName);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
}
