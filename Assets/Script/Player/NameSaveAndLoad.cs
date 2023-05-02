using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSaveAndLoad : MonoBehaviour
{
    private string _name;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _name = PlayerPrefs.GetString("name");
        GetComponent<TMP_InputField>().text = _name;
    }

   

    public void SaveName()
    {
        PlayerPrefs.SetString("name",GetComponent<TMP_InputField>().text);
    }
}
