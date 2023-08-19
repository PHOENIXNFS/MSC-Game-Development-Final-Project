using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimerAI : MonoBehaviour
{
    public bool bInputsEnabled;

    private void Start()
    {
        bInputsEnabled = false;
        StartCoutdownTimerCoroutine();
    }

    public void StartCoutdownTimerCoroutine()
    {
            StartCoroutine(RaceCountdown());
    }

    public IEnumerator RaceCountdown()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 11)
            yield return new WaitForSeconds(8f);

        if (SceneManager.GetActiveScene().buildIndex == 12 || SceneManager.GetActiveScene().buildIndex == 14 || SceneManager.GetActiveScene().buildIndex == 15)
            yield return new WaitForSeconds(1f);

        bInputsEnabled = true;

    }
}
