using System;
using UnityEngine;

public class supplyCheck : MonoBehaviour
{
    private float elapsedTime;
    private float stepTime = 1f;

    private void Start()
    {
        if (Minimap.instance != null)
        {
            Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.white, false, true, Minimap.IconStyle.SUPPLY);
        }
    }

    private void Update()
    {
        this.elapsedTime += Time.deltaTime;
        if (this.elapsedTime > this.stepTime)
        {
            this.elapsedTime -= this.stepTime;
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj2.GetComponent<HERO>() != null)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (Vector3.Distance(obj2.transform.position, base.transform.position) < 1.5f)
                        {
                            obj2.GetComponent<HERO>().getSupply();
                        }
                    }
                    else if (obj2.GetPhotonView().isMine && (Vector3.Distance(obj2.transform.position, base.transform.position) < 1.5f))
                    {
                        obj2.GetComponent<HERO>().getSupply();
                    }
                }
            }
        }
    }
}

