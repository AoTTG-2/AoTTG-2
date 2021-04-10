using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

public class supplyCheck : MonoBehaviour
{
    private float elapsedTime;
    private float stepTime = 1f;

    private void Start()
    {

    }

    private void Update()
    {
        this.elapsedTime += Time.deltaTime;
        if (this.elapsedTime > this.stepTime)
        {
            this.elapsedTime -= this.stepTime;
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj2.GetComponent<Hero>() != null)
                {
                    if (obj2.GetPhotonView().isMine && (Vector3.Distance(obj2.transform.position, base.transform.position) < 1.5f))
                    {
                        obj2.GetComponent<Hero>().GetSupply();
                    }
                }
            }
        }
    }
}

