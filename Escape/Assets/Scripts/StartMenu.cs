using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject settingsPanel;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Settings()
    {
        startPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void Back()
    {
        startPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
