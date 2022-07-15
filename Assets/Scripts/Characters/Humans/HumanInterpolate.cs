using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interpolates a human's rotation.
/// This exists because Unity's built-in rigidbody interpolation will not interpolate rotation due to physics for human rotation being disabled.
/// 
/// </summary>
public class HumanInterpolate : MonoBehaviour
{
    private Quaternion previousRotation;
    private Quaternion currentRotation;
    private Quaternion interpolatedRotation;
    private bool firstUpdateAfterFixedUpdate;
    private bool shouldInterpolate;

    private void Start()
    {
        firstUpdateAfterFixedUpdate = false;
        shouldInterpolate = true;
        previousRotation = GetComponent<Rigidbody>().rotation;
        currentRotation = GetComponent<Rigidbody>().rotation;
    }
    private void LateUpdate()
    {
        if (firstUpdateAfterFixedUpdate)
        {
            previousRotation = currentRotation;
            currentRotation = GetComponent<Rigidbody>().rotation;
            GetComponent<Rigidbody>().MoveRotation(previousRotation);
        }
        if (shouldInterpolate && previousRotation != currentRotation)
        {
            interpolatedRotation = Quaternion.Lerp(previousRotation, currentRotation, (Time.time - Time.fixedTime) / Time.fixedDeltaTime);
            GetComponent<Rigidbody>().MoveRotation(interpolatedRotation);
        }
    }
    private void FixedUpdate()
    {
        firstUpdateAfterFixedUpdate = true;
    }
    /// <summary>
    /// You must use this function instead of Rigidbody.MoveRotation() or transform.rotation = . Otherwise interpolation will break.
    /// (You could in theory use Rigidbody.MoveRotation() during FixedUpdate, but it's changes will be overriden by any calls to this function).
    /// </summary>
    /// <param name="newRotation"></param>
    public void SetRotationAtFixedUpdate(Quaternion newRotation)
    {
        previousRotation = interpolatedRotation;
        currentRotation = newRotation;
        StartCoroutine(nameof(WaitUntilFixedUpdate));
    }
    private IEnumerable WaitUntilFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        GetComponent<Rigidbody>().MoveRotation(currentRotation);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
