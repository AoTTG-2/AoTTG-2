using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldCannon
{
    [RequireComponent(typeof(PhotonView), typeof(SphereCollider))]
    public sealed class CannonBall : Photon.MonoBehaviour
    {
        private Cannon cannon;

        private Collider coll;

        private Vector3 correctPos, correctVelocity;

        private bool disabled;

        private int heroViewId;

        private List<TitanTrigger> myTitanTriggers;

        private int
            playerAttackBoxLayer,
            groundLayer,
            baseMask,
            groundMask;

        private Rigidbody rb;

        [SerializeField]
        private float smoothingDelay = 10f;

        private bool IsCollider
        {
            get { return coll.enabled; }
            set { coll.enabled = value; }
        }

        public static CannonBall Create(Vector3 position, Quaternion rotation, Vector3 velocity, Cannon cannon,
            int heroViewId)
        {
            var instance = PhotonNetwork.Instantiate("RC Resources/RC Prefabs/CannonBallObject",
                position,
                rotation,
                0).GetComponent<CannonBall>();
            instance.rb.velocity = velocity;
            instance.cannon = cannon;
            instance.heroViewId = heroViewId;
            return instance;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(GetComponent<Rigidbody>().velocity);
            }
            else
            {
                correctPos = (Vector3) stream.ReceiveNext();
                correctVelocity = (Vector3) stream.ReceiveNext();
            }
        }

        private void Awake()
        {
            myTitanTriggers = new List<TitanTrigger>();

            playerAttackBoxLayer = LayerMask.NameToLayer("PlayerAttackBox");
            groundLayer = LayerMask.NameToLayer("Ground");

            baseMask = 1 << playerAttackBoxLayer | 1 << LayerMask.NameToLayer("EnemyBox");
            groundMask = 1 << groundLayer;

            photonView.observed = this;

            correctPos = transform.position;
            correctVelocity = Vector3.zero;

            rb = GetComponent<Rigidbody>();
            coll = GetComponent<SphereCollider>();
            coll.enabled = false;

            if (photonView.isMine)
                StartCoroutine(WaitAndSelfDestruct(10f));
        }

        private void FixedUpdate()
        {
            if (photonView.isMine && !disabled)
            {
                var mask = IsCollider ? baseMask : (baseMask | groundMask);
                var hitColliders = Physics.OverlapSphere(transform.position, 0.6f, mask);
                var hitSelf = false;
                for (var i = 0; i < hitColliders.Length; i++)
                {
                    var currentGobj = hitColliders[i].gameObject;
                    if (currentGobj.layer == playerAttackBoxLayer)
                    {
                        var titanTrigger = currentGobj.GetComponent<TitanTrigger>();
                        if (!myTitanTriggers.Contains(titanTrigger))
                        {
                            titanTrigger.SetCollision(true);
                            myTitanTriggers.Add(titanTrigger);
                        }
                    }
                    else if (currentGobj.layer == groundLayer && currentGobj.GetComponentInParent<Cannon>() == cannon)
                        hitSelf = true; // We're assuming Cannon is groundLayer to avoid calling GetComponent.
                }

                if (!(IsCollider || hitSelf))
                    IsCollider = true;
            }
        }

        private void KillEnemyPlayersInRange(float range)
        {
            foreach (Hero player in FengGameManagerMKII.instance.getPlayers())
            {
                var isOtherPlayerWithinRange = player && !player.photonView.isMine && Vector3.Distance(player.transform.position, transform.position) <= range;
                if (isOtherPlayerWithinRange)
                {
                    var owner = player.gameObject.GetPhotonView().owner;

                    // TODO: Investigate whether all valid teams are > -1.
                    var playerTeam = (int) (owner.CustomProperties[PhotonPlayerProperty.RCteam] ?? -1);
                    var myTeam = (int) (PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam] ?? -1);
                    var bothTeamsValid = playerTeam != -1 && myTeam != -1;
                    var myTeamExists = myTeam != 0;
                    var sameTeam = myTeam == playerTeam;
                    var teamsEnabled = FengGameManagerMKII.Gamemode.Settings.TeamMode != TeamMode.Disabled;
                    var canKillPlayer = !(teamsEnabled && bothTeamsValid && myTeamExists && sameTeam);

                    if (canKillPlayer)
                        KillPlayer(player);
                }
            }
        }

        private void KillPlayer(Hero player)
        {
            var myName = $"{PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name] ?? "ERROR"} ";
            player.markDie();
            player.photonView.RPC(nameof(player.netDie2), PhotonTargets.All, -1, myName);
            FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!photonView.isMine)
                return;

            var collider = collision.collider;
            var isEnemyBoxLayer = collider.gameObject.layer == 10;
            if (isEnemyBoxLayer)
            {
                var titan = collision.gameObject.GetComponent<MindlessTitan>();
                if (titan != null)
                {
                    titan.photonView.RPC(nameof(titan.OnCannonHitRpc), titan.photonView.owner, heroViewId, collider.name);
                    SelfDestruct();
                }
            }
            else
                SelfDestruct();
        }

        private void ResetTitanTriggers()
        {
            for (var i = 0; i < myTitanTriggers.Count; i++)
                myTitanTriggers[i].SetCollision(false);
        }

        private void SelfDestruct()
        {
            if (disabled)
                return;

            disabled = true;

            // TODO: Replace this with a factory method.
            foreach (var collider in PhotonNetwork.Instantiate("FX/boom4", transform.position, transform.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>())
                collider.dmg = 0;

            if (FengGameManagerMKII.Gamemode.Settings.PvpCannons)
                KillEnemyPlayersInRange(20f);

            ResetTitanTriggers();

            PhotonNetwork.Destroy(gameObject);
        }

        private void Update()
        {
            if (!photonView.isMine)
            {
                transform.position = Vector3.Lerp(transform.position, correctPos, Time.deltaTime * smoothingDelay);
                rb.velocity = correctVelocity;
            }
        }

        private IEnumerator WaitAndSelfDestruct(float time)
        {
            yield return new WaitForSeconds(time);
            SelfDestruct();
        }
    }
}