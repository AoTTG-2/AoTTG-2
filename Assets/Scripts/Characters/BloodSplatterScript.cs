using System;
using UnityEngine;

public class BloodSplatterScript : MonoBehaviour
{
    private GameObject[] bloodInstances;
    public int bloodLocalRotationYOffset;
    public Transform bloodPosition;
    public Transform bloodPrefab;
    public Transform bloodRotation;
    public int maxAmountBloodPrefabs = 20;

    public void Main()
    {
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.bloodRotation.Rotate((float) 0f, (float) this.bloodLocalRotationYOffset, (float) 0f);
            Transform transform1 = UnityEngine.Object.Instantiate(this.bloodPrefab, this.bloodPosition.position, this.bloodRotation.rotation) as Transform;
            this.bloodInstances = GameObject.FindGameObjectsWithTag("blood");
            if (this.bloodInstances.Length >= this.maxAmountBloodPrefabs)
            {
                UnityEngine.Object.Destroy(this.bloodInstances[0]);
            }
        }
    }
}

