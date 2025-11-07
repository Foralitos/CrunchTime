using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour
{
    public GameObject pauseUI;
    
        public void OnGameResumePress()
    {
        
        pauseUI.SetActive(false);
    }

    public void OnExitGamePress()
    {
        
        Application.Quit();
    }

    public void OnEnterPausePress()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = false;
            isActive = !isActive;
            pauseUI.SetActive(isActive);
        }
    }

    void Update()
    {
        OnEnterPausePress();
    }
}
