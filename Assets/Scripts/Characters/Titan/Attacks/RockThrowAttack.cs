using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class RockThrowAttack : Attack
    {
        public RockThrowAttack()
        {
            BodyParts = new[] {BodyPart.ArmRight};
        }
        private GameObject Rock { get; set; }
        private bool UsedRock { get; set; }
        private string attackAnimation = "attack_throw";

        public override bool CanAttack(MindlessTitan titan)
        {
            if (!FengGameManagerMKII.Gamemode.Settings.PunkRockThrow) return false;
            if (IsDisabled(titan)) return false;
            var distance = Vector3.Distance(titan.transform.position, titan.Target.transform.position);
            if (distance < 100 && !titan.Animation.IsPlaying(attackAnimation)) return false;
            return true;
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished)
            {
                return;
            }

            if (!titan.Animation.IsPlaying(attackAnimation))
            {
                titan.CrossFade(attackAnimation, 0.1f);
                return;
            }

            if (titan.IsDisabled(BodyParts))
            {
                IsFinished = true;
                return;
            }

            if (!UsedRock && (titan.Animation[attackAnimation].normalizedTime >= 0.11f && titan.Animation[attackAnimation].normalizedTime <= 1f))
            {
                UsedRock = true;
                Transform transform = titan.TitanBody.HandRight;
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && titan.photonView.isMine)
                {
                    Rock = PhotonNetwork.Instantiate("FX/rockThrow", transform.position, transform.rotation, 0);
                }
                else
                {
                    Rock = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/rockThrow"), transform.position, transform.rotation);
                }
                
                var rockThrow = Rock.GetComponent<RockThrow>();
                Rock.transform.localScale = titan.transform.localScale;
                Transform transform1 = Rock.transform;
                transform1.position -= (Vector3)((Rock.transform.forward * 2.5f) * titan.Size);
                if (Rock.GetComponent<EnemyfxIDcontainer>() != null)
                {
                    Rock.GetComponent<EnemyfxIDcontainer>().titanName = titan.name;
                }
                Rock.transform.parent = transform;
                if (titan.photonView.isMine)
                {
                    rockThrow.photonView.RPC<int, Vector3, Vector3, float>(
                        rockThrow.initRPC,
                        PhotonTargets.Others,
                        titan.photonView.viewID,
                        titan.transform.localScale,
                        Rock.transform.localPosition,
                        titan.Size);
                }
            }
            if (titan.Animation[attackAnimation].normalizedTime >= 0.11f && titan.Animation[attackAnimation].normalizedTime <= 1f)
            {
                float y = Mathf.Atan2(titan.Target.transform.position.x - titan.transform.position.x, titan.Target.transform.position.z - titan.transform.position.z) * 57.29578f;
                titan.gameObject.transform.rotation = Quaternion.Euler(0f, y, 0f);
            }
            if ((Rock != null) && ((titan.Animation[attackAnimation].normalizedTime >= 0.62f && titan.Animation[attackAnimation].normalizedTime <= 1f)))
            {
                Vector3 vector6;
                float num3 = 1f;
                float num4 = -20f;
                if (titan.Target != null)
                {
                    vector6 = ((Vector3)((titan.Target.transform.position - Rock.transform.position) / num3)) + titan.Target.GetComponent<Rigidbody>().velocity;
                    float num5 = titan.Target.transform.position.y + (2f * titan.Size);
                    float num6 = num5 - Rock.transform.position.y;
                    vector6 = new Vector3(vector6.x, (num6 / num3) - ((0.5f * num4) * num3), vector6.z);
                }
                else
                {
                    vector6 = (Vector3)((titan.transform.forward * 60f) + (Vector3.up * 10f));
                }
                Rock.GetComponent<RockThrow>().launch(vector6);
                Rock.transform.parent = null;
                Rock = null;
            }

            if (titan.Animation[attackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
                UsedRock = false;
            }
        }
    }
}
