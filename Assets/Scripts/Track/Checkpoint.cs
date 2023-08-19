using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private TrackCheckpointManager trackCheckpointManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarControllerSimple>(out CarControllerSimple carControllerSimple))
        {
            trackCheckpointManager.CarPassedCheckpoint(this, other.transform);
        }

    }

    public void SetCurrentCheckpoint(TrackCheckpointManager trackCheckpointManager)
    {
        this.trackCheckpointManager = trackCheckpointManager;
    }
}
