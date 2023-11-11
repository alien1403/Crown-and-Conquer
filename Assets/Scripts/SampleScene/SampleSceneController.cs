using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSceneController : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public GameObject pauseMenuUI;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);

    }

    public void Options()
    {
        OptionPanel.alpha = 1.0f;
        OptionPanel.blocksRaycasts = true;
    }

    public void Back()
    {
        OptionPanel.alpha = 0.0f;
        OptionPanel.blocksRaycasts = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    void TogglePauseMenu()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
            EventSystemManager.Instance.SetEventSystemActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            EventSystemManager.Instance.SetEventSystemActive(true);
        }
    }
}
