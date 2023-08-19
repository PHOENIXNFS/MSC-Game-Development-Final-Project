using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;
using TMPro;

public class CarControllerAI : Agent
{
    [SerializeField] private TrackCheckpointManager trackCheckpointManager;
    [SerializeField] private Transform carSpawnPoint;
    //[SerializeField] private TMP_Text EpisodeCounter;

    [SerializeField] private CarControllerSimple carControllerSimple;
    //private int CountNumberOfEpisodes;

    [SerializeField] private CountdownTimerAI countdownTimerAI;

    [SerializeField] private float CurrentEpisodeReward;
    [SerializeField] private TMP_Text EpisodeRewardCounter;

    private void Awake()
    {
        carControllerSimple = GetComponent<CarControllerSimple>();
        //CountNumberOfEpisodes = 0;
        //EpisodeRewardCounter.text = CurrentEpisodeReward.ToString();
    }

    private void Start()
    {
        trackCheckpointManager.OnPlayerCorrectCheckpoint += CheckpointTracker_OnCorrectCheckpoint;
        trackCheckpointManager.OnPlayerWrongCheckpoint += CheckpointTracker_OnWrongCheckpoint;

        if (this.gameObject.tag == "AI Car")
            if(SceneManager.GetActiveScene().buildIndex == 4 || 
                SceneManager.GetActiveScene().buildIndex == 5 || 
                SceneManager.GetActiveScene().buildIndex == 11 || 
                SceneManager.GetActiveScene().buildIndex == 12 || 
                SceneManager.GetActiveScene().buildIndex == 14 || 
                SceneManager.GetActiveScene().buildIndex == 15)
                countdownTimerAI.StartCoutdownTimerCoroutine();

        CurrentEpisodeReward = 0f;
        EpisodeRewardCounter.text = CurrentEpisodeReward.ToString("F1");

    }

    private void CheckpointTracker_OnCorrectCheckpoint(object Sender, TrackCheckpointManager.CarCheckPointEventArgs e)
    {
        if (e.carTransform == transform)
        {
            AddReward(1f);
        }
    }

    private void CheckpointTracker_OnWrongCheckpoint(object Sender, TrackCheckpointManager.CarCheckPointEventArgs e)
    {
        if (e.carTransform == transform)
        {
            AddReward(-1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4 || 
            SceneManager.GetActiveScene().buildIndex == 5 || 
            SceneManager.GetActiveScene().buildIndex == 11)
        {
            if (transform.rotation.eulerAngles.y > -90f || transform.rotation.eulerAngles.y < -90f)
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        }

        if (SceneManager.GetActiveScene().buildIndex == 12 ||
            SceneManager.GetActiveScene().buildIndex == 14 ||
            SceneManager.GetActiveScene().buildIndex == 15)
        {
            transform.position = carSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            transform.forward = carSpawnPoint.forward;
            trackCheckpointManager.ResetCarChecpoint(transform);
            CurrentEpisodeReward = 0f;
            EpisodeRewardCounter.text = CurrentEpisodeReward.ToString("F1");
        }

        //For Training

        //transform.position = carSpawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        //transform.forward = carSpawnPoint.forward;
        //trackCheckpointManager.ResetCarChecpoint(transform);
        //carControllerSimple.StopVehicleCompletely();
        ////CountNumberOfEpisodes++;
        ////EpisodeCounter.text = CountNumberOfEpisodes.ToString();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (SceneManager.GetActiveScene().buildIndex == 4 || 
            SceneManager.GetActiveScene().buildIndex == 5 || 
            SceneManager.GetActiveScene().buildIndex == 11 || 
            SceneManager.GetActiveScene().buildIndex == 12 || 
            SceneManager.GetActiveScene().buildIndex == 14 || 
            SceneManager.GetActiveScene().buildIndex == 15)
        {
            Vector3 nextCheckpoint = trackCheckpointManager.GetNextCheckpoint(transform).transform.forward;
            float DirectionDot = Vector3.Dot(transform.forward, nextCheckpoint);
            sensor.AddObservation(DirectionDot);
        }

        //For Training
        //Vector3 nextCheckpoint = trackCheckpointManager.GetNextCheckpoint(transform).transform.forward;
        //float DirectionDot = Vector3.Dot(transform.forward, nextCheckpoint);
        //sensor.AddObservation(DirectionDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardmovementamount = 0f;
        float steeringangleamount = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0: forwardmovementamount = 0f; break;
            case 1: forwardmovementamount = 1f; break;
            case 2: forwardmovementamount = -1f; break;
        }

        switch (actions.DiscreteActions[1])
        {
            case 0: steeringangleamount = 0f; break;
            case 1: steeringangleamount = 1f; break;
            case 2: steeringangleamount = -1f; break;
        }

        // For Training
        //carControllerSimple.accelerationInput = forwardmovementamount;
        //carControllerSimple.steeringInput = steeringangleamount;

        if (countdownTimerAI.bInputsEnabled == true)
        {
            carControllerSimple.accelerationInput = forwardmovementamount;
            carControllerSimple.steeringInput = steeringangleamount;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardMovementAction = 0;
        if (Input.GetKey(KeyCode.W)) forwardMovementAction = 1;
        if (Input.GetKey(KeyCode.S)) forwardMovementAction = 2;

        int steeringMovementAction = 0;
        if (Input.GetKey(KeyCode.D)) steeringMovementAction = 1;
        if (Input.GetKey(KeyCode.A)) steeringMovementAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardMovementAction;
        discreteActions[1] = steeringMovementAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.5f);
        }

        if (collision.gameObject.tag == "Barrier")
        {
            AddReward(-0.5f);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.1f);
        }

        if (collision.gameObject.tag == "Barrier")
        {
            AddReward(-0.1f);
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Track Edge")
        {
            AddReward(-0.1f);
        }
    }

    public void AddReward(float reward)
    {
        CurrentEpisodeReward += reward;
        EpisodeRewardCounter.text = CurrentEpisodeReward.ToString("F1");
    }

}
