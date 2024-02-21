using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScripts : MonoBehaviour
{
    public static bool game_is_paused = false;
    [SerializeField] GameObject pause_menu;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  
        {
            if (game_is_paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pause_menu.SetActive(false);
        Time.timeScale = 1f;
        game_is_paused = false;
    }
    void Pause()
    {
        pause_menu.SetActive(true);
        Time.timeScale = 0f;
        game_is_paused = true;
    }
    public void Start_Again()
    {
        Time.timeScale = 1f;
        game_is_paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
