using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;






public class MainMenuController : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
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

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
        DataPersistenceManager.instance.DeleteSavedData();
        DataPersistenceManager.instance.NewGame();
    }

    public void Continue()
    {
        SceneManager.LoadScene("SampleScene");
        DataPersistenceManager.instance.LoadGame();
    }
}
