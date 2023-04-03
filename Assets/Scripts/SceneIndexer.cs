using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIndexer : MonoBehaviour
{
    public GameLoadedObject scriptableObject;

    public static SceneIndexer Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //Starts game without loading save file
    public void StartGame()
    {
        if(scriptableObject != null)
        {
            scriptableObject.newGame = true;
        }
        SceneManager.LoadScene("Arena");
    }

    //starts game using the save file
    public void LoadGame()
    {
        if (scriptableObject != null)
        {
            scriptableObject.newGame = false;
        }
        SceneManager.LoadScene("Arena");
    }

    //Proceeds to end scene
    public void EndScene()
    {
        SceneManager.LoadScene("End");
    }

    //Returns to title
    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    //quits application
    public void Exit()
    {
        Application.Quit();
    }


}