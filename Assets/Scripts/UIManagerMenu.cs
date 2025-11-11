using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour
{
    bool isActive = false;
    public GameObject pauseUI;
    
    public void OnGameResumePress()
    {
        pauseUI.SetActive(false);
        isActive = false;
        
        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnExitGamePress()
    {
        Application.Quit();
    }

    public void OnEnterPausePress()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            pauseUI.SetActive(isActive);
            
            if (isActive)
            {
                // Unlock cursor for menu interaction
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                // Lock cursor back for gameplay
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f; // Resume the game
            }
        }
    }

    void Update()
    {
        OnEnterPausePress();
    }
}