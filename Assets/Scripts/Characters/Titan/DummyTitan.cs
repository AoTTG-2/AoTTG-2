using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Characters;
using UnityEngine;


/// <summary>
/// The dummy titan. Rotates to find nearest player. Will switch players if another is closer after the focus time. Also can be disabled via hitting the ankle
/// </summary>
public class DummyTitan : TitanBase
{
    private readonly IPlayerService playerService = Service.Player;

    private MindlessTitanType mindlessType;
    public Transform pivot;
    private float timeTillRotate = 0f;
    private float timeTillRotateValue = 4f;
    private TextMesh healthLabel2;
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
        if (State == TitanState.Chase)
        {
            lookAtRotation = Quaternion.LookRotation(Target.transform.position - pivot.position);
            Vector3 desiredRotation = Quaternion.RotateTowards(pivot.rotation, lookAtRotation, speed * Time.deltaTime).eulerAngles;
            pivot.rotation = Quaternion.Euler(0, desiredRotation.y, 0);
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
        else if (timeTillRotate <= 0 && State == TitanState.Disabled)
        {
            photonView.RPC(nameof(ChangeState), PhotonTargets.All, TitanState.Idle);
            OnTargetRefresh();
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

        mindlessType = configuration.Type;
        name = mindlessType.ToString();

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

    public void AnkleHit(TitanHitEvent ev)
    {
        if (ev.PartHit == BodyPart.Ankle)
        {
            photonView.RPC(nameof(ChangeState), PhotonTargets.All, TitanState.Disabled);
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
    private void ChangeState(TitanState newState)
    {
        State = newState;

        if (State == TitanState.Disabled)
        {
            timeTillRotate = timeTillRotateValue;
        }
    }

    [PunRPC]
    protected override void OnDeath()
    {
        photonView.RPC(nameof(ChangeState), PhotonTargets.All, TitanState.Dead);
        SetStateAnimation(TitanState.Dead);
        Invoke(nameof(OnRecovering), 5.683f);
    }

    [PunRPC]
    protected override void OnRecovering()
    {
        photonView.RPC(nameof(ChangeState), PhotonTargets.All, TitanState.Recovering);
        SetStateAnimation(TitanState.Recovering);
        Invoke(nameof(ResetDummy), 3.125f);
    }
    private void ResetDummy()
    {
        photonView.RPC(nameof(ChangeState), PhotonTargets.All, TitanState.Idle);
        Health = MaxHealth;
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