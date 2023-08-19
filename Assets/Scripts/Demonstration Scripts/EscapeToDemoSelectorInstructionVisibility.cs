using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeToDemoSelectorInstructionVisibility : MonoBehaviour
{
    private void Update()
    {
        StartCoroutine(StartVisibilityTimer());
    }

    private IEnumerator StartVisibilityTimer()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
    }
}
