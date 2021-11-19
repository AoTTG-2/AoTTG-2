using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Characters;
using UnityEngine;


/// <summary>
/// The dummy titan. Hasn't been touched since May 2020, so should be updated to match the new standards
/// </summary>
public class DummyTitan : TitanBase
{
    private readonly IPlayerService playerService = Service.Player;

    public Transform pivot;
    private bool ankleEnabled = true;
    private float timeTillRotate = 0f;
    private float timeTillRotateValue = 4f;
    private TextMesh healthLabel2;
    public MinimapIcon minimapIcon;
    public Transform headPos;
    public MindlessTitanType MindlessType;

    public float speed = 3.0f;

    Quaternion lookAtRotation;

    private void Start()
    {
        playerService.OnTitanHit += AnkleHit;
        Initialize(new TitanConfiguration(0, 0, 0, MindlessTitanType.DummyTitan));
    }
    [PunRPC]
    private void Rotate()
    {
        lookAtRotation = Quaternion.LookRotation(Target.transform.position - pivot.position);
        Vector3 desiredRotation = Quaternion.RotateTowards(pivot.rotation, lookAtRotation, speed * Time.deltaTime).eulerAngles;
        pivot.rotation = Quaternion.Euler(0, desiredRotation.y, 0);
    }

    protected override void Update()
    {
        if (ankleEnabled && State == TitanState.Chase)
        {
            photonView.RPC(nameof(Rotate), PhotonTargets.All);
        }

        FocusTimer += Time.deltaTime;

        if (FocusTimer > Focus && FactionService.GetAllHostile(this).Count > 0)
        {
            OnTargetRefresh();
        }

        if (timeTillRotate > 0)
        {
            timeTillRotate -= Time.deltaTime;
        }
        else if (timeTillRotate <= 0 && (State != TitanState.Dead || State != TitanState.Recovering))
        {
            ankleEnabled = true;
        }
    }
    protected override void FixedUpdate() { }

    public override void Initialize(TitanConfiguration configuration)
    {
        State = TitanState.Idle;
        Health = configuration.Health;
        MaxHealth = configuration.Health;
        Size = configuration.Size;
        Focus = configuration.Focus;
        FocusTimer = configuration.Focus;
        ViewDistance = configuration.ViewDistance;

        MindlessType = configuration.Type;
        name = MindlessType.ToString();

        AnimationDeath = configuration.AnimationDeath;
        AnimationRecovery = configuration.AnimationRecovery;

        Body = gameObject.GetComponent<TitanBody>();
        Body.Head = headPos;

        HealthLabel = pivot.Find("BodyPivot/Body/HealthLabel").gameObject;
        healthLabel2 = pivot.Find("BodyPivot/Body/HealthLabel2").GetComponent<TextMesh>();

        transform.localScale = new Vector3(Size, Size, Size);

        photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);

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
            ankleEnabled = false;
            timeTillRotate = timeTillRotateValue;
        }        
    }

    protected override void OnTargetRefresh()
    {
        base.OnTargetRefresh();

        if (State == TitanState.Idle && TargetDistance < ViewDistance)
        {
            State = TitanState.Chase;
        }
        if (TargetDistance > ViewDistance)
        {
            State = TitanState.Idle;
        }
    }

    [PunRPC]
    protected override void OnDeath()
    {
        ankleEnabled = false;
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
        ankleEnabled = true;
        timeTillRotate = 0f;
        FocusTimer = Focus;
        photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
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