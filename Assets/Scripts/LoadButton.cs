using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    //Checks if save file already exists. If it doesn't then it sets the Load button to uninteractable
    void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/saveFile"))
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}