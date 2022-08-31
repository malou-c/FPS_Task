using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum ScenesEnum
{
    MENU,
    GAME
    
}

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene((int)ScenesEnum.GAME);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
