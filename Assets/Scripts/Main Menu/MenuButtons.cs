using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    private int IsTutorialCompleted;

    private void Awake()
    {
        IsTutorialCompleted = 0;
    }

    public void OpenTrackSelector()
    {
        if (PlayerPrefs.HasKey("IsTutorialCompleted") && PlayerPrefs.GetInt("IsTutorialCompleted") == 1)
            SceneManager.LoadScene(4);

        else
            SceneManager.LoadScene(9);
    }
    
    public void OpenGameManual()
    {
        SceneManager.LoadScene(2);
    }

    public void ReturnBackToMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void SelectOvalTrack()
    {
        SceneManager.LoadScene(5);
    }

    public void SelectNightTrack()
    {
        SceneManager.LoadScene(6);
    }

    public void SelectOvalObstacleTrack()
    {
        SceneManager.LoadScene(11);
    }

    public void SelectOvalTrackDemo()
    {
        SceneManager.LoadScene(14);
    }

    public void SelectNightTrackDemo()
    {
        SceneManager.LoadScene(15);
    }

    public void SelectOvalObstacleTrackDemo()
    {
        SceneManager.LoadScene(12);    
    }

    public void ExitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void ReturnToTrackSelect()
    {
        SceneManager.LoadScene(4);
    }

    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene(3);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenAIDemonstrationTrackSelector()
    {
        SceneManager.LoadScene(13);
    }

}
