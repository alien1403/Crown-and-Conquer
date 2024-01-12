using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneController : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public GameObject pauseMenuUI;
    public GameObject inventoryUI;
    private bool isPauseMenuActive = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        ToggleInventory();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        inventoryUI.SetActive(true); 
        isPauseMenuActive = false;
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
            inventoryUI.SetActive(true);
            EventSystemManager.Instance.SetEventSystemActive(false);
            isPauseMenuActive = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            inventoryUI.SetActive(false); 
            EventSystemManager.Instance.SetEventSystemActive(true);
            isPauseMenuActive = true;
        }
    }

    void ToggleInventory()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene" && isPauseMenuActive == false)
        {
            inventoryUI.SetActive(true); 
        }
        else
        {
            inventoryUI.SetActive(false);
        }
    }
}
