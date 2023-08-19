using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TrackCheckpointManager : MonoBehaviour
{
    public event EventHandler<CarCheckPointEventArgs> OnPlayerCorrectCheckpoint;
    public event EventHandler<CarCheckPointEventArgs> OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> RacersTransformList;
    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointIndexList;
    private List<int> racerCurrentLap;
    private List<int> racerPassedCheckpoints;
    private List<int> racerCurrentPositions;
    private int TotalCars;
    public int TotalLaps;
    [SerializeField] private TMP_Text PlayerTotalLapUIText;
    [SerializeField] private TMP_Text PlayerCurrentLapUI;
    [SerializeField] private TMP_Text PlayerCurrentPositionUI;
    [SerializeField] private TMP_Text PlayerTotalPositionUIText;
    [SerializeField] private TMP_Text PlayerTotalRaceTimeMinutes;
    [SerializeField] private TMP_Text PlayerTotalRaceTimeSeconds;
    //[SerializeField] private TMP_Text PlayerTotalRaceTimeMilliSeconds;
    private float RacerTimer = 0f;
    private bool bIsRaceTimerOn;


    private void Awake()
    {
        Transform CheckpointTransforms = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();

        PlayerTotalLapUIText.text = TotalLaps.ToString();

        TotalCars = RacersTransformList.Count;
        PlayerTotalPositionUIText.text = TotalCars.ToString();

        foreach (Transform CheckpointTransform in CheckpointTransforms)
        {
            Checkpoint checkpoint = CheckpointTransform.GetComponent<Checkpoint>();
            checkpoint.SetCurrentCheckpoint(this);
            checkpointList.Add(checkpoint);            
            
        }

        //Debug.Log(checkpointList.Count);

        nextCheckpointIndexList = new List<int>();
        foreach (Transform racerTransform in RacersTransformList)
        {
            nextCheckpointIndexList.Add(0);
        }

        racerCurrentLap = new List<int>();
        foreach (Transform racerTransform in RacersTransformList)
        {
            racerCurrentLap.Add(0);
        }

        racerPassedCheckpoints = new List<int>();
        foreach (Transform racerTransform in RacersTransformList)
        {
            racerPassedCheckpoints.Add(0);
        }

        racerCurrentPositions = new List<int>();
        int racerPositionCounter = 0;
        foreach (Transform racerTransform in RacersTransformList)
        {
            racerCurrentPositions.Add(racerPositionCounter + 1);
            racerPositionCounter++;
        }

        //Player Position System Updates After The cars passes First Checkpoint. First Checkpoint Starts the Race!
        PlayerCurrentPositionUI.text = racerCurrentPositions[0].ToString();
        bIsRaceTimerOn = false;
    }

    public void Update()
    {
        if (bIsRaceTimerOn)
        {
            RacerTimer = RacerTimer + Time.deltaTime;
            float Minutes = MathF.Floor(RacerTimer / 60);
            float Seconds = Mathf.Floor(RacerTimer % 60);
            if (Seconds == 60f)
            {
                Debug.Log("Reached 60 Seconds");
                Seconds -= 60f;
            }
            PlayerTotalRaceTimeMinutes.text = Minutes.ToString("00");
            PlayerTotalRaceTimeSeconds.text = Seconds.ToString("00");
        }
    }

    //Called when each car passes a checkpoint
    public void CarPassedCheckpoint(Checkpoint checkpoint, Transform racerTransform) 
    {
        int nextCheckpointIndex = nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)];
        int currentCarPassedCheckpointCount = racerPassedCheckpoints[RacersTransformList.IndexOf(racerTransform)]; 

        CarCheckPointEventArgs e = new CarCheckPointEventArgs
        {
            carTransform = racerTransform
        };

        // Car Passes Correct Checkpoint
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex) 
        {

            //Updates Next Checkpoint
            nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;

            //Updates How Many Checkpoints This Car Has Passed
            racerPassedCheckpoints[RacersTransformList.IndexOf(racerTransform)] = currentCarPassedCheckpointCount + 1;

            //Updates Position System of This Car
            CarPositionUpdate(racerTransform);

            //Current Position of Player
            PlayerCurrentPositionUI.text = racerCurrentPositions[0].ToString();

            if (racerTransform.gameObject.tag == "AI Car")
                OnPlayerCorrectCheckpoint?.Invoke(this, e); //Training Rules For Correct Checkpoint

            //For checking Laps
            if (checkpointList.IndexOf(checkpoint) == 0)
            {
                //Invokes Lap Checking Method For This Car
                CarLapSystem(racerTransform);
            }
        }

        // Car Passes InCorrect Checkpoint
        else
        {
            if (racerTransform.gameObject.tag == "AI Car")
                OnPlayerWrongCheckpoint?.Invoke(this, e); // Training Rules For Wrong Checkpoint
        }

    }

    public void CarPositionUpdate(Transform racerTransform)
    {
        if (racerCurrentPositions[RacersTransformList.IndexOf(racerTransform)] > 1)
        {
            Transform currentCarTransform = racerTransform;
            int currentCarPosition = racerCurrentPositions[RacersTransformList.IndexOf(racerTransform)];
            int currentCarCheckpointsPassed = racerPassedCheckpoints[RacersTransformList.IndexOf(racerTransform)];

            Transform carInFrontTransform = null;
            int carInFrontPosition = 0;
            int carInFrontCheckpointsPassed = 0;

            for (int i = 0; i < TotalCars; i++)
            {
                if (racerCurrentPositions[i] == currentCarPosition - 1)
                {
                    carInFrontTransform = RacersTransformList[i];
                    carInFrontPosition = racerCurrentPositions[i];
                    carInFrontCheckpointsPassed = racerPassedCheckpoints[i];
                    break;
                }
            }

            //Checks which car is ahead based on number of checkpoints passed
            if (currentCarCheckpointsPassed > carInFrontCheckpointsPassed)
            {
                racerCurrentPositions[RacersTransformList.IndexOf(currentCarTransform)] = currentCarPosition - 1;
                racerCurrentPositions[RacersTransformList.IndexOf(carInFrontTransform)] = carInFrontPosition + 1;
            }

            // Checks which car is ahead based on distance to next checkpoint if both of them have passed same number of checkpoints
            if (currentCarCheckpointsPassed == carInFrontCheckpointsPassed)
            {
                float distancetoNextCheckpointCurrentCar = Vector3.Distance(currentCarTransform.position, GetNextCheckpoint(currentCarTransform).transform.position);
                float distancetoNextCheckpointCarInFront = Vector3.Distance(carInFrontTransform.position, GetNextCheckpoint(carInFrontTransform).transform.position);

                if (distancetoNextCheckpointCurrentCar < distancetoNextCheckpointCarInFront)
                {
                    racerCurrentPositions[RacersTransformList.IndexOf(currentCarTransform)] = currentCarPosition - 1;
                    racerCurrentPositions[RacersTransformList.IndexOf(carInFrontTransform)] = carInFrontPosition + 1;
                }
            }
        }
    }

    public void CarLapSystem(Transform racerTransform)
    {
        int nextLap = racerCurrentLap[RacersTransformList.IndexOf(racerTransform)];
        racerCurrentLap[RacersTransformList.IndexOf(racerTransform)] = nextLap + 1;

        //Starts Race Timer
        if (racerCurrentLap[RacersTransformList.IndexOf(racerTransform)] == 1 && racerTransform.gameObject.tag == "Player")
        {
            bIsRaceTimerOn = true;
        }

        //Updates Player UI For Laps Completed
        if (racerCurrentLap[RacersTransformList.IndexOf(racerTransform)] < TotalLaps + 1 && racerTransform.gameObject.tag == "Player")
        {
            PlayerCurrentLapUI.text = racerCurrentLap[RacersTransformList.IndexOf(racerTransform)].ToString();
        }

        //Handles both AI and Player upon Completion of all laps of Race
        if (racerCurrentLap[RacersTransformList.IndexOf(racerTransform)] == TotalLaps + 1)
        {
            if (racerTransform.gameObject.tag == "AI Car")
            {
                if(!(SceneManager.GetActiveScene().buildIndex == 12 || SceneManager.GetActiveScene().buildIndex == 14 || SceneManager.GetActiveScene().buildIndex == 15))
                    racerTransform.gameObject.SetActive(false);
            }
            if (racerTransform.gameObject.tag == "Player")
            {
                bIsRaceTimerOn = false;

                if (racerCurrentPositions[RacersTransformList.IndexOf(racerTransform)] == 1)
                {
                    if (SceneManager.GetActiveScene().buildIndex == 9)
                    {
                        Destroy(this);
                        int bTutorialCompleted = 1;

                        PlayerPrefs.SetInt("IsTutorialCompleted", bTutorialCompleted);
                        SceneManager.LoadScene(10);
                    }


                    for (int i = 1; i <= 3; i++)
                    {
                        Destroy(RacersTransformList[i].gameObject);
                    }
                    Destroy(this);
                    SceneManager.LoadScene(7);
                }

                else
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        Destroy(RacersTransformList[i].gameObject);
                    }
                    Destroy(this);
                    SceneManager.LoadScene(8);
                }
                //UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }


    public class CarCheckPointEventArgs : EventArgs
    {
        public Transform carTransform { get; set; }
    }

    public void ResetCarChecpoint(Transform racerTransform)
    {
        nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)] = 0;
    }

    //Obtains transform of next checkpoint for the particular Car
    public Transform GetNextCheckpoint(Transform racerTransform) 
    {
        int nextCheckpointIndex = nextCheckpointIndexList[RacersTransformList.IndexOf(racerTransform)];

        Transform CheckpointTransforms = transform.Find("Checkpoints");

        Transform NextCheckpointTransform = null;

        int count = 0;

        foreach (Transform CheckpointTransform in CheckpointTransforms)
        {
            if (count == nextCheckpointIndex)
            {
                NextCheckpointTransform = CheckpointTransform;
                break;
            }

            else
                count++;
        }

        return NextCheckpointTransform;

    }

    public void PlayerRestartLevel()
    {
        for (int i = 1; i <= 3; i++)
        {
            Destroy(RacersTransformList[i].gameObject);
        }
        //Destroy(this);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerReturnToMainMenu()
    {
        for (int i = 1; i <= 3; i++)
        {
            Destroy(RacersTransformList[i].gameObject);
        }
        //Destroy(this);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }


}
