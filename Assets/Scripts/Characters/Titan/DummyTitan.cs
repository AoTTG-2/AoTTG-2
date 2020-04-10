using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTitan : Photon.MonoBehaviour
{
    public int health = 300;
    private FengGameManagerMKII MultiplayerManager;
    public GameObject myHero;
    public Transform pivot;
    public bool dead = false;
    public bool canRotate = true;
    public TextMesh healthLabel;
    public TextMesh healthLabel2;

    public float speed = 3.0f;

    Quaternion lookAtRotation;

    private void Start()
    {
        this.MultiplayerManager = FengGameManagerMKII.instance;
        pivot = transform.Find("BodyPivot");
        healthLabel = pivot.Find("Body/HealthLabel").gameObject.GetComponent<TextMesh>();
        healthLabel2 = pivot.Find("Body/HealthLabel2").gameObject.GetComponent<TextMesh>();
    }

    void Update()
    {
        if(canRotate)
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
        healthLabel.text = healthLabel2.text = health.ToString();
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

    [PunRPC]
    public void GetHit(int dmg)
    {
        if(health != 0)
        {
            if(dmg >= health)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        if (!dead)
        {
            Transform body = pivot.transform.Find("Body");
            body.transform.parent = null;
            Rigidbody rb = body.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = 15;
            body.GetComponent<MeshCollider>().convex = true;

            Destroy(body.gameObject, 3);
            Destroy(gameObject, 3);
            dead = true;
        }
    }
}