using UnityEngine;
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
            pauseUI.SetActive(true);
        }
    }

    void Update()
    {
        OnEnterPausePress();
    }
}
