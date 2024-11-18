using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public static SceneManger S_instance;

    void Awake()
    {
       
        if (S_instance == null)
        {
            S_instance = this;
        }
        else if(S_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    public void NextGameScene()
    {
        SceneManager.LoadScene("LoddingScene");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }
    public void NextLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
