using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interpolates a human's rotation.
/// This exists because Unity's built-in rigidbody interpolation will not interpolate rotation due to physics for human rotation being disabled.
/// </summary>
public class HumanInterpolate : MonoBehaviour
{
    private Quaternion previousRotation;
    private Quaternion currentRotation;
    private Quaternion interpolatedRotation;
    private bool firstUpdateAfterFixedUpdate;
    private bool shouldSetRotation;

    private bool shouldSetPosition;
    private Vector3 positionToSet;

    private void Start()
    {
        firstUpdateAfterFixedUpdate = false;
        shouldSetRotation = false;
        shouldSetPosition = false;
        previousRotation = GetComponent<Rigidbody>().rotation;
        currentRotation = GetComponent<Rigidbody>().rotation;
        StartCoroutine(nameof(WaitUntilAfterFixedUpdate));
    }
    private void LateUpdate()
    {
        if (firstUpdateAfterFixedUpdate)
        {
            previousRotation = currentRotation;
            currentRotation = GetComponent<Rigidbody>().rotation;
            GetComponent<Rigidbody>().MoveRotation(previousRotation);
        }
        if (previousRotation != currentRotation)
        {
            interpolatedRotation = Quaternion.Lerp(previousRotation, currentRotation, (Time.time - Time.fixedTime) / Time.fixedDeltaTime);
            GetComponent<Rigidbody>().MoveRotation(interpolatedRotation);
        }
    }
    private void FixedUpdate()
    {
        Debug.Log("<color=blue>Fixed Update</color>");
        firstUpdateAfterFixedUpdate = true;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private IEnumerator WaitUntilAfterFixedUpdate()
    {
        while (true)
        {
            Debug.Log("<color=yellow>WaitingForFixedUpdate</color>");
            yield return new WaitForFixedUpdate();
            Debug.Log("<color=red>Executing WaitUntilAfterFixedUpdate</color>");
            if (shouldSetPosition)
            {
                Debug.Log($"Rigidbdy position set to: {positionToSet}");
                GetComponent<Rigidbody>().MovePosition(positionToSet);
                shouldSetPosition = false;
            }
            if (shouldSetRotation)
            {
                Debug.Log($"Rigidbody rotation set to: {currentRotation.eulerAngles}");
                GetComponent<Rigidbody>().MoveRotation(currentRotation);
                shouldSetRotation = false;
            }
            //This might seem overcomplicated, but don't touch it!
            //Since rotation is frozen, Rigidbody.rotation is updated to transform.rotation when physics are simulated (immediately after FixedUpdate())
            //We can't set transform.rotation directly without breaking Unity's rigidbody interpolation.
            //So we have to call a coroutine that sets the rigidbody rotation after the physics are simulated.
        }
    }
    #region SetTransformAtFixedUpdate overloads
    /// <summary>
    /// You must use this function instead of Rigidbody.MoveRotation() or transform.rotation = . Otherwise interpolation will break.
    /// A null argument will not change the position/rotation.
    /// </summary>
    /// <param name="newRotation"></param>
    public void SetTransformAtFixedUpdate(Vector3 newPosition, Quaternion newRotation)
    {
        SetTransformAtFixedUpdate(newPosition);
        SetTransformAtFixedUpdate(newRotation);
    }
    public void SetTransformAtFixedUpdate(Vector3 newPosition)
    {
        positionToSet = newPosition;
        shouldSetPosition = true;
    }
    public void SetTransformAtFixedUpdate(Quaternion newRotation)
    {
        previousRotation = interpolatedRotation;
        currentRotation = newRotation;
        shouldSetRotation = true;
    }
    #endregion
}
