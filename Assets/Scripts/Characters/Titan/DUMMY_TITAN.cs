using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DUMMY_TITAN : Photon.MonoBehaviour
{
    public int health = 1500;
    private FengGameManagerMKII MultiplayerManager;
    public GameObject myHero;
    public Transform pivot;
    public bool dead = false;

    public float speed = 3.0f;

    Quaternion lookAtRotation;

    private void Start()
    {
        this.MultiplayerManager = FengGameManagerMKII.instance;
        pivot = transform.Find("BodyPivot");
    }

    void Update()
    {
        if (myHero)
        {
            lookAtRotation = Quaternion.LookRotation(myHero.transform.position - pivot.position);
            Vector3 desiredRotation = Quaternion.RotateTowards(pivot.rotation, lookAtRotation, speed * Time.deltaTime).eulerAngles;
            pivot.rotation = Quaternion.Euler(0, desiredRotation.y, 0);
        }
        else
        {
            myHero = GetNearestHero();
        }
    }

    private GameObject GetNearestHero()
    {
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        foreach (Hero hero in this.MultiplayerManager.getPlayers())
        {
            GameObject gameObject = hero.gameObject;
            float num2 = Vector3.Distance(gameObject.transform.position, transform.position);
            if (num2 < positiveInfinity)
            {
                obj2 = gameObject;
                positiveInfinity = num2;
            }
        }
        return obj2;
    }

    public void Die()
    {
        if (!dead)
        {
            Transform body = pivot.transform.Find("Body");
            body.transform.parent = null;
            body.gameObject.AddComponent<Rigidbody>().useGravity = true;
            body.GetComponent<MeshCollider>().convex = true;

            Destroy(body.gameObject, 3);
            Destroy(gameObject, 3);
            dead = true;
        }
    }
}