using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform VehicleTarget;
    [SerializeField] private float CameraMovementSpeed;
    [SerializeField] private float CameraRotationSpeed;
    [SerializeField] private Vector3 CameraOffset;

    private void FixedUpdate()
    {
        HandleCameraMovement();
        HandleCameraRotation();
    }

    private void HandleCameraMovement()
    {
        var targetCameraPosition = VehicleTarget.TransformPoint(CameraOffset);
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, CameraMovementSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        var targetCameraDirection = VehicleTarget.position - transform.position;
        var targetCameraRotation = Quaternion.LookRotation(targetCameraDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetCameraRotation, CameraRotationSpeed * Time.deltaTime);
    }
}
