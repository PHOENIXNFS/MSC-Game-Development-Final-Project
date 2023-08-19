using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameTutorial : MonoBehaviour
{
    public GameObject WelcomeUIPart1;
    public AnimationClip WelcomeUIPart1Opening;
    public AnimationClip WelcomeUIPart1Closing;
    private Animator WelcomeUIPart1Animator;
    private bool bIsWelcomePart1Finished;

    public GameObject WelcomeUIPart2;
    public AnimationClip WelcomeUIPart2Opening;
    public AnimationClip WelcomeUIPart2Closing;
    private Animator WelcomeUIPart2Animator;
    private bool bIsWelcomePart2Finished;

    public GameObject AccelerationUI;
    public AnimationClip AccelerationOpening;
    public AnimationClip AccelerationClosing;
    private Animator AccelerationAnimator;
    private bool bIsAccelerationUIFinished;

    public GameObject BrakeUI;
    public AnimationClip BrakeOpening;
    public AnimationClip BrakeClosing;
    private Animator BrakeAnimator;
    private bool bIsBrakeUIFinished;

    public GameObject TurnRightUI;
    public AnimationClip TurnRightOpening;
    public AnimationClip TurnRightClosing;
    private Animator TurnRightAnimator;
    private bool bIsTurnRightUIFinished;

    public GameObject TurnLeftUI;
    public AnimationClip TurnLeftOpening;
    public AnimationClip TurnLeftClosing;
    private Animator TurnLeftAnimator;
    private bool bIsTurnLeftUIFinished;

    public GameObject TutorialEndPart1;
    public AnimationClip TutorialEndPart1Opening;
    public AnimationClip TutorialEndPart1Closing;
    private Animator TutorialEndPart1Animator;
    private bool bIsTutorialEndPart1Finished;

    public GameObject TutorialEndPart2;
    public AnimationClip TutorialEndPart2Opening;
    public AnimationClip TutorialEndPart2Closing;
    private Animator TutorialEndPart2Animator;
    private bool bIsTutorialEndPart2Finished;

    public GameObject TutorialEndPart3;
    public AnimationClip TutorialEndPart3Opening;
    public AnimationClip TutorialEndPart3Closing;
    private Animator TutorialEndPart3Animator;
    private bool bIsTutorialEndPart3Finished;

    public GameObject PauseMenu;

    private bool bIsGameWaitingForAccelerationInput;
    private bool bIsGameWaitingForBrakeInput;
    private bool bIsGameWaitingForTurnRightInput;
    private bool bIsGameWaitingForTurnLeftInput;

    private void Start()
    {
        WelcomeUIPart1Animator = WelcomeUIPart1.GetComponent<Animator>();
        WelcomeUIPart2Animator = WelcomeUIPart2.GetComponent<Animator>();
        AccelerationAnimator = AccelerationUI.GetComponent<Animator>();
        BrakeAnimator = BrakeUI.GetComponent<Animator>();
        TurnRightAnimator = TurnRightUI.GetComponent<Animator>();
        TurnLeftAnimator = TurnLeftUI.GetComponent<Animator>();
        TutorialEndPart1Animator = TutorialEndPart1.GetComponent<Animator>();
        TutorialEndPart2Animator = TutorialEndPart2.GetComponent<Animator>();
        TutorialEndPart3Animator = TutorialEndPart3.GetComponent<Animator>();

        bIsWelcomePart1Finished = false;
        bIsWelcomePart2Finished = false;
        bIsAccelerationUIFinished = false;
        bIsBrakeUIFinished = false;
        bIsTurnRightUIFinished = false;
        bIsTurnLeftUIFinished = false;
        bIsTutorialEndPart1Finished = false;
        bIsTutorialEndPart2Finished = false;
        bIsTutorialEndPart3Finished = false;

        WelcomeUIPart1.SetActive(false);
        WelcomeUIPart2.SetActive(false);
        AccelerationUI.SetActive(false);
        BrakeUI.SetActive(false);
        TurnRightUI.SetActive(false);
        TurnLeftUI.SetActive(false);
        TutorialEndPart1.SetActive(false);
        TutorialEndPart2.SetActive(false);
        TutorialEndPart3.SetActive(false);

        bIsGameWaitingForAccelerationInput = false;
        bIsGameWaitingForBrakeInput = false;
        bIsGameWaitingForTurnRightInput = false;
        bIsGameWaitingForTurnLeftInput = false;

        PauseMenu.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!bIsWelcomePart1Finished && !WelcomeUIPart1.activeSelf)
            StartCoroutine(WelcomeUIPart1Tutorial());

        if(bIsWelcomePart1Finished && !bIsWelcomePart2Finished && !WelcomeUIPart2.activeSelf)
            StartCoroutine(WelcomeUIPart2Tutorial());

        if (bIsWelcomePart2Finished && !bIsAccelerationUIFinished && !AccelerationUI.activeSelf)
            StartCoroutine(AccelerationUITutorial());

        if (bIsAccelerationUIFinished && !bIsTurnRightUIFinished && !TurnRightUI.activeSelf)
            StartCoroutine(TurnRightUITutorial());

        if (bIsTurnRightUIFinished && !bIsBrakeUIFinished && !BrakeUI.activeSelf)
            StartCoroutine(BrakeUITutorial());

        if (bIsBrakeUIFinished && !bIsTurnLeftUIFinished && !TurnLeftUI.activeSelf)
            StartCoroutine(TurnLeftUITutorial());

        if (bIsTurnLeftUIFinished && !bIsTutorialEndPart1Finished && !TutorialEndPart1.activeSelf)
            StartCoroutine(EndPart1Tutorial());

        if(bIsTutorialEndPart1Finished && !bIsTutorialEndPart2Finished && !TutorialEndPart2.activeSelf)
            StartCoroutine(EndPart2Tutorial());

        if (bIsTutorialEndPart2Finished && !bIsTutorialEndPart3Finished && !TutorialEndPart3.activeSelf)
            StartCoroutine(EndPart3Tutorial());


    }

    private IEnumerator WelcomeUIPart1Tutorial()
    {
        WelcomeUIPart1.SetActive(true);

        WelcomeUIPart1.transform.localScale = Vector3.zero;

        yield return RunAnimation(WelcomeUIPart1Animator, WelcomeUIPart1Opening);

        yield return new WaitForSeconds(4f);

        yield return RunAnimation(WelcomeUIPart1Animator, WelcomeUIPart1Closing);

        yield return new WaitForSeconds(2f);

        WelcomeUIPart1.SetActive(false);

        bIsWelcomePart1Finished = true;
    }

    private IEnumerator WelcomeUIPart2Tutorial()
    {
        WelcomeUIPart2.SetActive(true);

        WelcomeUIPart2.transform.localScale = Vector3.zero;

        yield return RunAnimation(WelcomeUIPart2Animator, WelcomeUIPart2Opening);

        yield return new WaitForSeconds(4f);

        yield return RunAnimation(WelcomeUIPart2Animator, WelcomeUIPart2Closing);

        WelcomeUIPart2.SetActive(false);

        bIsWelcomePart2Finished = true;
    }

    private IEnumerator AccelerationUITutorial()
    {
        AccelerationUI.SetActive(true);

        AccelerationUI.transform.localScale = Vector3.zero;

        yield return RunAnimation(AccelerationAnimator, AccelerationOpening);

        bIsGameWaitingForAccelerationInput = true;

        yield return WaitForAccelerationInput();

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(AccelerationAnimator, AccelerationClosing);

        AccelerationUI.SetActive(false);

        bIsAccelerationUIFinished = true;
    }

    private IEnumerator BrakeUITutorial()
    {
        BrakeUI.SetActive(true);

        BrakeUI.transform.localScale = Vector3.zero;

        yield return RunAnimation(BrakeAnimator, BrakeOpening);

        bIsGameWaitingForBrakeInput = true;

        yield return WaitForBrakeInput();

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(BrakeAnimator, BrakeClosing);

        BrakeUI.SetActive(false);

        bIsBrakeUIFinished = true;

    }

    private IEnumerator TurnRightUITutorial()
    {
        TurnRightUI.SetActive(true);

        TurnRightUI.transform.localScale = Vector3.zero;

        yield return RunAnimation(TurnRightAnimator, TurnRightOpening);

        bIsGameWaitingForTurnRightInput = true;

        yield return WaitForTurnRightInput();

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(TurnRightAnimator, TurnRightClosing);

        TurnRightUI.SetActive(false);

        bIsTurnRightUIFinished = true;
    }

    private IEnumerator TurnLeftUITutorial()
    {
        TurnLeftUI.SetActive(true);

        TurnRightUI.transform.localScale = Vector3.zero;

        yield return RunAnimation(TurnLeftAnimator, TurnLeftOpening);

        bIsGameWaitingForTurnLeftInput = true;

        yield return WaitForTurnLeftInput();

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(TurnLeftAnimator, TurnLeftClosing);

        TurnLeftUI.SetActive(false);

        bIsTurnLeftUIFinished = true;
    }

    private IEnumerator EndPart1Tutorial()
    {
        TutorialEndPart1.SetActive(true);

        TutorialEndPart1.transform.localScale = Vector3.zero;

        //PauseMenu.SetActive(true);

        yield return RunAnimation(TutorialEndPart1Animator, TutorialEndPart1Opening);

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(TutorialEndPart1Animator, TutorialEndPart1Closing);

        TutorialEndPart1.SetActive(false);

        bIsTutorialEndPart1Finished = true;

    }

    private IEnumerator EndPart2Tutorial()
    {
        TutorialEndPart2.SetActive(true);

        TutorialEndPart2.transform.localScale = Vector3.zero;

        //PauseMenu.SetActive(true);

        yield return RunAnimation(TutorialEndPart2Animator, TutorialEndPart2Opening);

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(TutorialEndPart2Animator, TutorialEndPart2Closing);

        TutorialEndPart2.SetActive(false);

        bIsTutorialEndPart2Finished = true;

    }

    private IEnumerator EndPart3Tutorial()
    {
        TutorialEndPart3.SetActive(true);

        TutorialEndPart3.transform.localScale = Vector3.zero;

        //PauseMenu.SetActive(true);

        yield return RunAnimation(TutorialEndPart3Animator, TutorialEndPart3Opening);

        yield return new WaitForSeconds(5f);

        yield return RunAnimation(TutorialEndPart3Animator, TutorialEndPart3Closing);

        TutorialEndPart3.SetActive(false);

        bIsTutorialEndPart3Finished = true;

    }

    private IEnumerator RunAnimation(Animator animator, AnimationClip Clip)
    {
        animator.Play(Clip.name);

        yield return new WaitForSeconds(Clip.length); 
    }

    private IEnumerator WaitForAccelerationInput()
    {
        while(bIsGameWaitingForAccelerationInput)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                bIsGameWaitingForAccelerationInput = false;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForBrakeInput()
    {
        while (bIsGameWaitingForBrakeInput)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                bIsGameWaitingForBrakeInput = false;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForTurnRightInput()
    {
        while (bIsGameWaitingForTurnRightInput)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                bIsGameWaitingForTurnRightInput = false;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForTurnLeftInput()
    {
        while (bIsGameWaitingForTurnLeftInput)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                bIsGameWaitingForTurnLeftInput = false;
            }

            yield return null;
        }
    }

}
