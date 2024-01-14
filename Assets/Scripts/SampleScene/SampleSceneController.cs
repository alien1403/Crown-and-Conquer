using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneController : MonoBehaviour
{
    public static SampleSceneController _instance { get; private set; }

    public CanvasGroup OptionPanel;
    public GameObject pauseMenuUI;
    public GameObject inventoryPanel;
    private bool isPauseMenuActive = false;

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

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
            EventSystemManager.Instance.SetEventSystemActive(false);
            isPauseMenuActive = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            EventSystemManager.Instance.SetEventSystemActive(true);
            isPauseMenuActive = true;
        }
    }

    public void ToggleInventoryPanel(bool value)
    {
        if (!pauseMenuUI.activeSelf)
        {
            inventoryPanel.SetActive(value);
        }
    }
}
