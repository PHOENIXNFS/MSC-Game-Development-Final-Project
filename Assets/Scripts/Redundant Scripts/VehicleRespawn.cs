using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRespawn : MonoBehaviour
{
    [SerializeField] Transform VehicleTarget;

    private void FixedUpdate()
    {
        HandleVehicleRespawn();
    }
    private void HandleVehicleRespawn()
    {
        if (VehicleTarget.rotation.eulerAngles.z >= 85f || VehicleTarget.rotation.eulerAngles.z <= -85f)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Vector3 eulerRotation = VehicleTarget.rotation.eulerAngles;
                VehicleTarget.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0f);
                //Debug.Log($"rot {VehicleTarget.rotation}");
            }
        }
    }
}
