using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _playButton;
    void Start()
    {

    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RunGame()
    {
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
        SetActivePlayButton(true);
        gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene((int)ScenesEnum.MENU);
    }

    public void SetActivePlayButton(bool newActive)
    {
        _playButton.SetActive(newActive);
    }
}
