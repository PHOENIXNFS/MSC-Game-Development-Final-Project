using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] float IntroSceneWaitTime;    

    void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    public IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(IntroSceneWaitTime);

        SceneManager.LoadScene(1);

    }
}
