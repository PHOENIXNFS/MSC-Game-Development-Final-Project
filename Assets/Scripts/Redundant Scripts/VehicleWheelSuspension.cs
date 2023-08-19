using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWheelSuspension : MonoBehaviour
{
    public float suspensionRange = 0.2f; // Maximum distance the wheel can move up or down.
    public float suspensionSpeed = 10f; // Speed at which the suspension moves.

    private Vector3 initialPosition; // Initial position of the wheel.
    private bool isOverSpeedbreaker = false; // Flag to check if the wheel is over a speedbreaker.

    private void Start()
    {
        initialPosition = transform.localPosition; // Store the initial position of the wheel.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Speedbreaker"))
        {
            isOverSpeedbreaker = true; // Set the flag to true when the wheel collides with a speedbreaker.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Speedbreaker"))
        {
            isOverSpeedbreaker = false; // Set the flag to false when the wheel exits the speedbreaker.
        }
    }

    private void Update()
    {
        // If the wheel is over a speedbreaker, apply suspension effect.
        if (isOverSpeedbreaker)
        {
            // Calculate the target position based on the suspension range and the initial position.
            Vector3 targetPosition = initialPosition - transform.up * suspensionRange;

            // Move the wheel towards the target position using Lerp for a smooth effect.
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * suspensionSpeed);
        }
        else
        {
            // If the wheel is not over a speedbreaker, reset it to its initial position.
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * suspensionSpeed);
        }
    }
}
