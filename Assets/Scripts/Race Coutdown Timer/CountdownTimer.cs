using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text RaceCountDownTimer;

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
        //yield return new WaitForSeconds(0.5f);
        RaceCountDownTimer.text = "3";
        //RaceCountDownTimer.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        //RaceCountDownTimer.gameObject.SetActive(false);
        RaceCountDownTimer.text = "2";
        //RaceCountDownTimer.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        //RaceCountDownTimer.gameObject.SetActive(false);
        RaceCountDownTimer.text = "1";
        //RaceCountDownTimer.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        RaceCountDownTimer.text = "GO!";

        yield return new WaitForSeconds(2f);
        bInputsEnabled = true;
        RaceCountDownTimer.gameObject.SetActive(false);

    }

}
