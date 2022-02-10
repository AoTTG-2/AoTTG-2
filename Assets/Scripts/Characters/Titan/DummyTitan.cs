using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.InGame.HUD;
using UnityEngine;
using System.Collections;

/// <summary>
/// The dummy titan. Hasn't been touched since May 2020, so should be updated to match the new standards
/// </summary>
public class DummyTitan : Photon.MonoBehaviour
{
    protected readonly IEntityService EntityService = Service.Entity;

    public int health = 300;
    public GameObject myHero;
    public Transform pivot;
    public bool dead = false;
    private float deathTimer = 15f;
    public bool canRotate = true;
    //public TextMesh healthLabel;
    //public TextMesh healthLabel2;
    public MinimapIcon minimapIcon;

    public AudioSource dummyWiggleNoise;
    private float wiggleSoundTimer = 3f;
    public AudioSource hitNoise;
    public AudioSource fallNoise;
    public AudioSource standNoise;

    private bool kneeHit = false;
    private float rotationTimer = 5f;

    public float speed = 3.0f;

    public Animator dummyAnimator;

    Quaternion lookAtRotation;

    private void Start()
    {
        pivot = transform.Find("BodyPivot");
        //healthLabel = pivot.Find("Body/HealthLabel").gameObject.GetComponent<TextMesh>();
        //healthLabel2 = pivot.Find("Body/HealthLabel2").gameObject.GetComponent<TextMesh>();
    }

    void Update()
    {

        if(dead == false && canRotate == true)
        {
            Debug.Log("wiggleSound timer is running");
            WiggleSound(wiggleSoundTimer);
        }
        if (canRotate)
        {
            if (myHero && kneeHit == false)
            {
                Rotation();
            }
            else if(myHero && kneeHit == true)
            {
                Debug.Log("Timer is running");
                StartCoroutine(KneeHitTimer(rotationTimer));  
            }
            else
            {
                myHero = GetNearestHero();
            }
        }
        if (dead == true)
        {
            Debug.Log("Deathtimer is running");
            StartCoroutine(DeathTimer(deathTimer));
        }
        //healthLabel.text = healthLabel2.text = health.ToString();
    }

     private IEnumerator KneeHitTimer(float rotationTimer)
     {
        yield return new WaitForSeconds(rotationTimer);
        kneeHit = false;
     }

    private GameObject GetNearestHero()
    {
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        foreach (Hero hero in EntityService.GetAll<Hero>())
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
    public void GetHit(float dmg)
    {
        if(health != 0)
        {
            if(dmg >= health)
            {
                hitNoise.Play();
                Die();
            }
        }
    }

    private IEnumerator DeathTimer(float deathTimer)
    {
        yield return new WaitForSeconds(deathTimer);
        Revive();
    }

    private void Revive()
    {
        standNoise.Play();
        dead = false;
        dummyAnimator.SetTrigger("dummyReviveAnim");
    }

    public void Die()
    {
        dummyAnimator.SetTrigger("dummyDeathAnim");
        fallNoise.Play();
        dead = true;
    }

    private IEnumerator WiggleSound(float wigglesoundTimer)
    {
        yield return new WaitForSeconds(wigglesoundTimer);
        dummyWiggleNoise.Play();
    }

    public void KneeHit()
    {
        hitNoise.Play();
        kneeHit = true;
    }

    private void Rotation()
    {
        if(dead == false)
        {
            dummyAnimator.SetTrigger("dummyIdleAnim");
            lookAtRotation = Quaternion.LookRotation(myHero.transform.position - pivot.position);
            Vector3 desiredRotation = Quaternion.RotateTowards(pivot.rotation, lookAtRotation, speed * Time.deltaTime).eulerAngles;
            pivot.rotation = Quaternion.Euler(0, desiredRotation.y, 0);
        }
        
    }
}