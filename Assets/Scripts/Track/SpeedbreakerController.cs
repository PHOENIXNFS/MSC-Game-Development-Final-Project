using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedbreakerController : MonoBehaviour
{
    public float suspensionDuration = 0.5f;
    public float suspensionHeight = 0.2f;
    public float suspensionForce = 5000f;

    private bool isSpeedbreakerActive = false;

    public float speedReductionFactor = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the vehicle collides with the speedbreaker.
        if (collision.gameObject.CompareTag("AI Car") || collision.gameObject.CompareTag("Player"))
        {
            // Start the suspension effect.
            if (!isSpeedbreakerActive)
            {
                isSpeedbreakerActive = true;
                StartCoroutine(ApplySuspensionEffect(collision.transform));
            }
        }
    }

    private IEnumerator ApplySuspensionEffect(Transform vehicleTransform)
    {
        Rigidbody vehicleRigidbody = vehicleTransform.GetComponent<Rigidbody>();
        Vector3 originalVelocity = vehicleRigidbody.velocity;

        // Raise the vehicle by the specified suspensionHeight.
        vehicleTransform.position += Vector3.up * suspensionHeight;

        // Apply an upward force to the vehicle to simulate the suspension effect.
        vehicleRigidbody.AddForce(Vector3.up * suspensionForce, ForceMode.Impulse);

        // Reduce the vehicle's speed.
        float OriginalSpeed = vehicleTransform.gameObject.GetComponent<CarControllerSimple>().currentSpeed;
        vehicleTransform.gameObject.GetComponent<CarControllerSimple>().currentSpeed *= speedReductionFactor;

        // Wait for the specified duration.
        yield return new WaitForSeconds(suspensionDuration);

        // Restore the vehicle's position based on its current Rigidbody position.
        Vector3 newPosition = vehicleRigidbody.position;
        newPosition.y -= suspensionHeight;
        vehicleRigidbody.position = newPosition;

        isSpeedbreakerActive = false;
    }
}
