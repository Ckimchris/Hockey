using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class Manager : PersistableObject
{
    public bool debug;
    public GameLoadedObject scriptableObject;
    public PersistentStorage persistentStorage;
    public PlayerController player;
    public Sphere sphere;
    public List<Cube> cubes;
    public TextMeshProUGUI scoreText;
    public GameObject pauseScreen;
    public GameObject pauseButton;

    private float currentCount = 0;
    private float totalCount;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug bool for in editor testing. Allows us to test fresh
        if(!debug)
        {
            CheckSaveFile();
        }
        else
        {
            currentCount = 0;
        }


        SetUpUI();
    }

    //Listening to the cube hit event from all of the available cubes
    private void OnEnable()
    {
        for(int i = 0; i < cubes.Count; i++)
        {
            cubes[i].cubeHit += ValidateVictory;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].cubeHit -= ValidateVictory;
        }
    }

    private void SetUpUI()
    {
        totalCount = cubes.Count;

        if(scoreText != null)
        {
            scoreText.text = currentCount + "/" + totalCount;
        }
    }

    private void CheckSaveFile()
    {
        //Checks if its start or load. 
        if (!scriptableObject.newGame)
        {
            //Checks if save file exists and then loads
            if (File.Exists(Application.persistentDataPath + "/saveFile"))
            {
                persistentStorage.Load(this);

                for (int i = 0; i < cubes.Count; i++)
                {
                    if (cubes[i].GetComponent<Cube>().isHit)
                    {
                        currentCount++;
                    }
                }
            }
        }
        else
        {
            currentCount = 0;
        }
    }

    void UpdateScore()
    {
        currentCount++;

        if (scoreText != null)
        {
            scoreText.text = currentCount + "/" + totalCount;
        }
    }

    //Updates current score and ui. If current count equals total then it ends the game
    void ValidateVictory()
    {
        UpdateScore();
        if(currentCount == totalCount)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        SceneIndexer.Instance.EndScene();
    }

    public void Pause()
    {
        if(pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }

        if(pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        Time.timeScale = 0;
    }

    public void Unpause()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
        }

        Time.timeScale = 1;
    }

    public void SaveAndQuit()
    {
        persistentStorage.Save(this);
        Unpause();
        SceneIndexer.Instance.ReturnTitle();
    }

    public void ReturnTitle()
    {
        Unpause();
        SceneIndexer.Instance.ReturnTitle();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(cubes.Count);
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].Save(writer);
        }

        if(sphere != null)
        {
            sphere.Save(writer);
        }
        if(player != null)
        {
            player.Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            cubes[i].Load(reader);
        }

        if (sphere != null)
        {
            sphere.Load(reader);
        }
        if (player != null)
        {
            player.Load(reader);
        }
    }
}
