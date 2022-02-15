using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Characters.Titan.Configuration;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// The dummy titan. Rotates to find nearest player. Will switch players if another is closer after the focus time. Also can be disabled via hitting the ankle
    /// </summary>
    public class DummyTitan : TitanBase
    {
        public Transform pivot;
        private float timeTillRotate = 0f;
        private readonly float timeTillRotateValue = 4f;
        public Collider napeCollider;
        private TextMesh healthLabel2;
        public MinimapIcon minimapIcon;
        public Transform headPos;
        public float speed = 3.0f;

        private enum AudioState
        {
            Chasing,
            Falling,
            StandingUp,
            Hit,
        }
        public AudioSource wiggleNoise;
        public AudioSource fallDown;
        public AudioSource standUp;
        public AudioSource hitNoise;

        Quaternion lookAtRotation;

        // How I currently initialize the Initialize method. Not sure if there is a better way atm. Change if necessary.
        private void Start()
        {
            Initialize(new TitanConfiguration(0, 0, 0, MindlessTitanType.DummyTitan));
        }

        // Update Logic for Masterclient to dictate the behavior of the titan (only this because the spawner for the dummy titan spawns only on the masterclient).
        protected override void Update()
        {
            if (!photonView.isMine) return;

            if (State == TitanState.Chase && Target)
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
                State = TitanState.Idle;
                OnTargetRefresh();
            }

            if (State == TitanState.Dead || State == TitanState.Recovering)
            {
                napeCollider.enabled = false;
            }
            else
            {
                if (!napeCollider.enabled)
                    napeCollider.enabled = true;
            }

            if (State == TitanState.Chase && !wiggleNoise.isPlaying)
            {
                photonView.RPC(nameof(PlayAudio), PhotonTargets.All, AudioState.Chasing);
            }

        }

        // Plays the audio depending on what state the dummy titan is in
        [PunRPC]
        private void PlayAudio(AudioState state)
        {           
            switch (state)
            {
                case AudioState.Chasing:
                    wiggleNoise.Play();
                    break;
                case AudioState.Falling:
                    fallDown.Play();
                    break;
                case AudioState.Hit:
                    hitNoise.Play();
                    break;
                case AudioState.StandingUp:
                    standUp.Play();
                    break;
            }
        }

        protected override void FixedUpdate()
        {
            // Empty to override the FixedUpdate in BaseTitan.cs
        }

        // Overrdies the initialize on BaseTitan.cs. Sets up all the parameters that are needed for the Dummy to function
        public override void Initialize(TitanConfiguration configuration)
        {
            State = TitanState.Idle;
            Health = configuration.Health;
            MaxHealth = configuration.Health;
            Size = configuration.Size;
            Focus = configuration.Focus;
            FocusTimer = configuration.Focus;
            ViewDistance = configuration.ViewDistance * Size;

            var mindlessType = configuration.Type;
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

        // Finds a new target once the focus timer is reached. Also changes the state based on what is found.
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

        // The RPC used to change states. If anyone including host hits a dummy then this method will be called to update the state for the Host thus changing the Dummy's behavior
        [PunRPC]
        public void ChangeState(TitanState newState)
        {
            State = newState;

            if (State == TitanState.Disabled)
            {
                photonView.RPC(nameof(PlayAudio), PhotonTargets.All, AudioState.Hit);
                timeTillRotate = timeTillRotateValue;
            }
        }

        // Activates the Death animation and changes the state for client and host (if host didn't kill the dummy). Needed for Update logic etc. on Masterclient 
        protected override void OnDeath()
        {
            photonView.RPC(nameof(PlayAudio), PhotonTargets.All, AudioState.Falling);
            photonView.RPC(nameof(ChangeState), PhotonTargets.MasterClient, TitanState.Dead);
            SetStateAnimation(TitanState.Dead);
            Invoke(nameof(OnRecovering), 5.683f);
        }

        // Activates the Recovering Animation and changes the state for client and host (if host didn't kill the dummy). Needed for Update logic etc. on masterclient
        protected override void OnRecovering()
        {
            photonView.RPC(nameof(PlayAudio), PhotonTargets.All, AudioState.StandingUp);
            photonView.RPC(nameof(ChangeState), PhotonTargets.MasterClient, TitanState.Recovering);
            SetStateAnimation(TitanState.Recovering);
            Invoke(nameof(ResetDummy), 3.125f);
        }

        // Resets the Dummy to have his normal health and State while enabling the nape collider (keep from insta killing during recovering)
        private void ResetDummy()
        {
            ChangeState(TitanState.Idle);
            Health = MaxHealth;
            timeTillRotate = 0f;
            FocusTimer = Focus;
            photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
            napeCollider.enabled = true;
        }

        // Updates the HP label for both the client and Host side dummy's. Without this only the person who damaged the dummy would see the HP change.
        [PunRPC]
        protected override void UpdateHealthLabelRpc(int currentHealth, int maxHealth)
        {
            hitNoise.Play();
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
}
