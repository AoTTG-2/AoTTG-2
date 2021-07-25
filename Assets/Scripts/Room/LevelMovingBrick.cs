using System;
using UnityEngine;

/// <summary>
/// Moves a GameObject from <see cref="pointGOA"/> to <see cref="pointGOB"/> with <see cref="speed"/> units per second.
/// </summary>
public class LevelMovingBrick : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    public GameObject pointGOA;
    public GameObject pointGOB;
    public float speed = 10f;
    public bool towardsA = true;

    private void Start()
    {
        this.pointA = this.pointGOA.transform.position;
        this.pointB = this.pointGOB.transform.position;
        UnityEngine.Object.Destroy(this.pointGOA);
        UnityEngine.Object.Destroy(this.pointGOB);
    }

    private void Update()
    {
        if (this.towardsA)
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.pointA, this.speed * Time.deltaTime);
            if (Vector3.Distance(base.transform.position, this.pointA) < 2f)
            {
                this.towardsA = false;
            }
        }
        else
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.pointB, this.speed * Time.deltaTime);
            if (Vector3.Distance(base.transform.position, this.pointB) < 2f)
            {
                this.towardsA = true;
            }
        }
    }
}

