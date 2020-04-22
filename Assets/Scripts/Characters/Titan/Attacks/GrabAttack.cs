using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class GrabAttack : Attack
    {

        private string AttackAnimation { get; set; }
        private float attackCheckTimeA { get; set; }
        private float attackCheckTimeB { get; set; }
        private GameObject GrabbedTarget { get; set; }

        private TitanHand Hand { get; set; }

        private enum TitanHand
        {
            Left,
            Right
        }

        public override bool CanAttack(MindlessTitan titan)
        {
            if (titan.TargetDistance >= titan.AttackDistance) return false;

            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Vector3.Distance(titan.Target.transform.position, titan.transform.Find("chkOverHead").position) < (3.6f * titan.Size))
            {
                if (between > 0f)
                {
                    AttackAnimation = "grab_head_front_r";
                    Hand = TitanHand.Right;
                }
                else
                {
                    AttackAnimation = "grab_head_front_l";
                    Hand = TitanHand.Left;
                }
                attackCheckTimeA = 0.38f;
                attackCheckTimeB = 0.55f;
                return true;
            }
            
            if (between > 0f)
            {
                if (Vector3.Distance(titan.Target.transform.position, titan.transform.Find("chkBackRight").position) < (2.8f * titan.Size))
                {
                    AttackAnimation = "grab_head_back_r";
                    Hand = TitanHand.Right;
                    attackCheckTimeA = 0.45f;
                    attackCheckTimeB = 0.5f;
                    return true;
                }
            }
            if (Vector3.Distance(titan.Target.transform.position, titan.transform.Find("chkBackLeft").position) < (2.8f * titan.Size))
            {
                AttackAnimation = "grab_head_back_l";
                Hand = TitanHand.Left;
                attackCheckTimeA = 0.45f;
                attackCheckTimeB = 0.5f;
                return true;
            }

            if (Mathf.Abs(between) < 90f && titan.TargetDistance < (titan.AttackDistance * 0.5f))
            {
                AttackAnimation = between > 0f
                    ? "grab_ground_front_r"
                    : "grab_ground_front_l";
                Hand = between > 0f
                    ? TitanHand.Right
                    : TitanHand.Left;
                attackCheckTimeA = 0.37f;
                attackCheckTimeB = 0.6f;
                return true;
            }

            if (between > 0f && titan.TargetDistance < (titan.AttackDistance * 0.5f))
            {
                AttackAnimation = between > 0f
                    ? "grab_ground_back_r"
                    : "grab_ground_back_l";
                Hand = between > 0f
                    ? TitanHand.Right
                    : TitanHand.Left;
                attackCheckTimeA = 0.34f;
                attackCheckTimeB = 0.49f;
                return true;
            }
            return false;
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.Animation.CrossFade(AttackAnimation);
                return;
            }

            if (titan.Animation[AttackAnimation].normalizedTime >= this.attackCheckTimeA && titan.Animation[AttackAnimation].normalizedTime <= this.attackCheckTimeB/* && (this.grabbedTarget == null)*/)
            {
                var hand = Hand == TitanHand.Left
                    ? titan.TitanBody.HandLeft
                    : titan.TitanBody.HandRight;

                GameObject grabTarget = checkIfHitHand(hand, titan.Size);
                if (grabTarget != null)
                {
                    if (Hand == TitanHand.Left)
                    {
                        eatSetL(titan, grabTarget);
                        GrabbedTarget = grabTarget;
                    }
                    else
                    {
                        eatSet(titan, grabTarget);
                        GrabbedTarget = grabTarget;
                    }
                }
            }
            if (titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                if (GrabbedTarget != null)
                {
                    titan.OnTargetGrabbed(GrabbedTarget, Hand == TitanHand.Left);
                }
                IsFinished = true;
            }
        }

        private void eatSet(MindlessTitan titan, GameObject grabTarget)
        {
            if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || !titan.photonView.isMine)) || !grabTarget.GetComponent<Hero>().isGrabbed)
            {
                titan.grabToRight();
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && titan.photonView.isMine)
                {
                    titan.photonView.RPC("grabToRight", PhotonTargets.Others, new object[0]);
                    object[] parameters = new object[] { "grabbed" };
                    grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters);
                    object[] objArray2 = new object[] { titan.photonView.viewID, false };
                    grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray2);
                }
                else
                {
                    grabTarget.GetComponent<Hero>().grabbed(titan.gameObject, false);
                    grabTarget.GetComponent<Hero>().GetComponent<Animation>().Play("grabbed");
                }
            }
        }

        private void eatSetL(MindlessTitan titan, GameObject grabTarget)
        {
            if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || !titan.photonView.isMine)) || !grabTarget.GetComponent<Hero>().isGrabbed)
            {
                titan.grabToLeft();
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && titan.photonView.isMine)
                {
                    titan.photonView.RPC("grabToLeft", PhotonTargets.Others, new object[0]);
                    object[] parameters = new object[] { "grabbed" };
                    grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters);
                    object[] objArray2 = new object[] { titan.photonView.viewID, true };
                    grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, objArray2);
                }
                else
                {
                    grabTarget.GetComponent<Hero>().grabbed(titan.gameObject, true);
                    grabTarget.GetComponent<Hero>().GetComponent<Animation>().Play("grabbed");
                }
            }
        }
    }
}
