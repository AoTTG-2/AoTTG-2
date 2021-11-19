using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Characters;
using System.Collections.Generic;
using Assets.Scripts.Characters.Titan.Behavior;
using Newtonsoft.Json;
using UnityEngine;


/// <summary>
/// The dummy titan. Hasn't been touched since May 2020, so should be updated to match the new standards
/// </summary>
public class DummyTitan : TitanBase
{
    private readonly IPlayerService playerService = Service.Player;

    public GameObject myHero;
    public Transform pivot;
    public bool canRotate = true;
    private bool ankleEnabled = true;
    private float timeTillRotate = 0f;
    private float timeTillRotateValue = 4f;
    public TextMesh healthLabel2;
    public MinimapIcon minimapIcon;
    public Transform headPos;

    public float speed = 3.0f;

    Quaternion lookAtRotation;

    private void Start()
    {
        playerService.OnTitanHit += AnkleHit;
        Initialize(new TitanConfiguration(0, 0, 0, MindlessTitanType.DummyTitan));
    }

    protected override void Update()
    {
        if (canRotate)
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

        if (ankleEnabled && timeTillRotate > 0)
        {
            timeTillRotate -= Time.deltaTime;
        }
        else if (ankleEnabled && timeTillRotate <= 0)
        {
            canRotate = true;
        }

        //healthLabel2.text = HealthLabel.GetComponent<TextMesh>().text;

    }
    protected override void FixedUpdate() { }

    public override void Initialize(TitanConfiguration configuration)
    {
        Type = TitanType.DummyTitan;
        State = TitanState.Idle;
        Health = configuration.Health;
        MaxHealth = configuration.Health;
        Size = configuration.Size;
        AnimationDeath = configuration.AnimationDeath;
        AnimationRecovery = configuration.AnimationRecovery;
        Body = gameObject.GetComponent<TitanBody>();
        Body.Head = headPos;
        HealthLabel = pivot.Find("BodyPivot/Body/HealthLabel").gameObject;
        healthLabel2 = pivot.Find("BodyPivot/Body/HealthLabel2").GetComponent<TextMesh>();

        transform.localScale = new Vector3(Size, Size, Size);

        if (photonView.isMine)
        {
            configuration.Behaviors = new List<TitanBehavior>();
            var config = JsonConvert.SerializeObject(configuration);
            photonView.RPC(nameof(InitializeRpc), PhotonTargets.OthersBuffered, config);

            if (Health > 0)
            {
                photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
            }
        }

        //EntityService.Register(this);
    }

    [PunRPC]
    public void InitializeRpc(string titanConfiguration, PhotonMessageInfo info)
    {
        if (info.sender.ID == photonView.ownerId)
        {
            Initialize(JsonConvert.DeserializeObject<TitanConfiguration>(titanConfiguration));
        }
    }

    public override void OnHit(Entity attacker, int damage)
    {
        base.OnHit(attacker, damage);
    }

    [PunRPC]
    public void AnkleHit(TitanHitEvent ev)
    {
        if (ev.PartHit == BodyPart.Ankle)
        {
            canRotate = false;
            timeTillRotate = timeTillRotateValue;
        }

        
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
    protected override void OnDeath()
    {
        canRotate = false;
        ankleEnabled = false;

        //CrossFade(AnimationDeath, 0f);
        Invoke("OnRecovering", 5.683f);
    }

    [PunRPC]
    protected override void OnRecovering()
    {
        State = TitanState.Recovering;
        SetStateAnimation(TitanState.Recovering);
        Invoke("ResetDummy", 3.125f);
    }
    private void ResetDummy()
    {
        State = TitanState.Idle;
        Health = MaxHealth;
        canRotate = true;
        ankleEnabled = true;
        timeTillRotate = 0f;
        photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
        HealthLabel = pivot.Find("BodyPivot/Body/HealthLabel").gameObject;

    }

    [PunRPC]
    protected override void UpdateHealthLabelRpc(int currentHealth, int maxHealth)
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        
        var color = "7FFF00";
        var num2 = ((float) currentHealth) / ((float) maxHealth);
        if ((num2 < 0.75f) && (num2 >= 0.5f))
        {
            color = "f2b50f";
        }
        else if ((num2 < 0.5f) && (num2 >= 0.25f))
        {
            color = "ff8100";
        }
        else if (num2 < 0.25f)
        {
            color = "ff3333";
        }
        HealthLabel.GetComponent<TextMesh>().text = $"<color=#{color}>{currentHealth}</color>";

        healthLabel2.text = HealthLabel.GetComponent<TextMesh>().text;
    }


}