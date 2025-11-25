using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour
{
    bool isActive = false;
    public GameObject pauseUI;
    public MonoBehaviour playerCameraScript; // Assign your camera control script here

    void Start()
    {
        // Asegurar que el juego NO inicie en pausa
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
            isActive = false;
        }

        // Asegurar configuración inicial del cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Asegurar que el tiempo esté corriendo
        Time.timeScale = 1f;
    }

    public void OnGameResumePress()
    {
        pauseUI.SetActive(false);
        isActive = false;
        
        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Resume game
        Time.timeScale = 1f;
        
        // Re-enable camera control
        if (playerCameraScript != null)
            playerCameraScript.enabled = true;
    }

    public void OnExitGamePress()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
                
                // Pause game
                Time.timeScale = 0f;
                
                // Disable camera control
                if (playerCameraScript != null)
                    playerCameraScript.enabled = false;
            }
            else
            {
                // Lock cursor back for gameplay
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
                // Resume game
                Time.timeScale = 1f;
                
                // Re-enable camera control
                if (playerCameraScript != null)
                    playerCameraScript.enabled = true;
            }
        }
    }

    void Update()
    {
        OnEnterPausePress();
    }
}