using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CarControllerSimple : MonoBehaviour
{
    private const string HORIZONTAL_MOTION = "Horizontal";
    private const string VERTICAL_MOTION = "Vertical";

    public float maxSpeed = 10f; // Maximum speed of the car
    public float acceleration = 5f; // Acceleration force
    public float deceleration = 10f; // Deceleration force
    public float deceleration_drag = 0.1f;
    public float turnSpeed = 200f; // Turning speed
    public float wheelBase = 2f; // Distance between front and rear axles
    public float angularDamping = 2f; // Angular damping for smoothing
    public float maxTurnAngle = 30f; // Maximum steering angle

    public Transform frontLeftWheel; // Reference to the front left wheel object
    public Transform frontRightWheel; // Reference to the front right wheel object

    public Transform[] CarWheelTransforms;

    public float currentSpeed; // Current speed of the car
    private float currentSteerAngle; // Current steering angle
    private float previousSteerInput; // Previous steering input

    public float accelerationInput;
    public float steeringInput;
    public float slowdownRate;

    [SerializeField] private TMP_Text PlayerCarSpeed;
    [SerializeField] private GameObject PauseMenu;

    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private bool bIsInputsEnabledInTutorial;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PauseMenu.SetActive(false);
    }

    private void Start()
    {
        if (this.gameObject.tag == "Player" && SceneManager.GetActiveScene().buildIndex != 9)
            countdownTimer.StartCoutdownTimerCoroutine();

        bIsInputsEnabledInTutorial = false;

        if (this.gameObject.tag == "Player" && SceneManager.GetActiveScene().buildIndex == 9)
            StartCoroutine(TutorialCountdown());
    }

    private void Update()
    {
        if (this.gameObject.tag == "Player")
        {
            PlayerPauseMenuOpen();
        }
    }

    private void FixedUpdate()
    {
        if(this.gameObject.tag == "Player")
        {
            if (countdownTimer.bInputsEnabled == true && SceneManager.GetActiveScene().buildIndex != 9)
            {
                GetInput();
                PlayerCarSpeed.text = (rb.velocity.magnitude * 5).ToString("0");
            }

            else
            {
                if (bIsInputsEnabledInTutorial == true)
                {
                    GetInput();
                    PlayerCarSpeed.text = (rb.velocity.magnitude * 5).ToString("0");
                }
               
            }
            
            
        }
        //For AI Input during Inference Learing Demo Record
        //GetInput();
        CarLocomotion();
    }

    public void GetInput()
    {
        // Get input for acceleration, braking, and steering
        accelerationInput = Input.GetAxis(VERTICAL_MOTION);
        steeringInput = Input.GetAxis(HORIZONTAL_MOTION);

    }

    private void CarLocomotion()
    {
        // Calculate the current speed based on acceleration and deceleration
        if (accelerationInput > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (accelerationInput < 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed, deceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration_drag * Time.deltaTime);
        }

        // Calculate the steering angle based on steering input and clamp it
        currentSteerAngle = Mathf.Clamp(steeringInput * turnSpeed, -maxTurnAngle, maxTurnAngle);

        // Calculate the rotation angle for the front wheels
        float frontWheelRotation = currentSteerAngle * Mathf.Deg2Rad;

        // Calculate the turn radius
        float turnRadius = wheelBase / Mathf.Sin(frontWheelRotation);

        // Calculate the angular velocity required for the car to turn at the desired radius
        float targetAngularVelocity = currentSpeed / turnRadius;

        // Apply the rotation to the front wheels
        frontLeftWheel.localRotation = Quaternion.Euler(0f, currentSteerAngle, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(0f, currentSteerAngle, 0f);

        foreach (Transform wheel in CarWheelTransforms)
        {
            wheel.Rotate(Vector3.right, currentSpeed * Mathf.Rad2Deg * Time.deltaTime);
        }

        // Calculate the movement vector based on the current speed and the car's forward direction
        Vector3 movement = transform.forward * currentSpeed;

        // Apply the movement to the rigidbody
        rb.velocity = movement;

        // Apply the turning force to the rigidbody
        rb.angularVelocity = new Vector3(0f, targetAngularVelocity, 0f);

        // Smooth out the angular velocity and steering angle when transitioning from turning to straight
        if (steeringInput == 0f && previousSteerInput != 0f)
        {
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, angularDamping * Time.deltaTime);
            currentSteerAngle = Mathf.Lerp(currentSteerAngle, 0f, angularDamping * Time.deltaTime);
        }

        previousSteerInput = steeringInput;
    }

    public void StopVehicleCompletely()
    {
        if (rb.velocity.magnitude > 0f )
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            currentSpeed = 0f;
            currentSteerAngle = 0f;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Track Edge"))
        {
            if (currentSpeed >= 6f)
            { 
                float slowdownFactor = 1f - slowdownRate * Time.deltaTime;

                currentSpeed *= slowdownFactor;

                if (currentSpeed < 6f)
                {
                    currentSpeed = 6f;
                }
                
            }

            if (currentSpeed <= -6f)
            {
                float slowdownFactor = 1f - slowdownRate * Time.deltaTime;

                currentSpeed *= slowdownFactor;

                if (currentSpeed > -6f)
                {
                    currentSpeed = -6f;
                }

            }
        }
    }

    public void PlayerPauseMenuOpen()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void PlayerPauseMenuClose()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PlayerPauseMenuQuitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public IEnumerator TutorialCountdown()
    {
        yield return new WaitForSeconds(15f);
        bIsInputsEnabledInTutorial = true;
    }

}
