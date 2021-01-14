using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class JeanSkill : Skill
    {
        public JeanSkill(Hero hero) : base(hero)
        {
        }

        public int TimesUsed { get; set; }

        private const int TimesAllowed = 1;

        //if ((this.state == HERO_STATE.Grab) && !this.useGun)
        //{
        //    if (this.skillId == "jean")
        //    {
        //        if (((this.state != HERO_STATE.Attack) &&
        //             (InputManager.KeyDown(InputHuman.Attack) ||
        //              InputManager.KeyDown(InputHuman.AttackSpecial))) &&
        //            ((this.escapeTimes > 0) && !this.Animation.IsPlaying("grabbed_jean")))
        //        {
        //            this.playAnimation("grabbed_jean");
        //            this.Animation["grabbed_jean"].time = 0f;
        //            this.escapeTimes--;
        //        }
        //        if ((this.Animation.IsPlaying("grabbed_jean") && (this.Animation["grabbed_jean"].normalizedTime > 0.64f)) && (this.titanWhoGrabMe.GetComponent<MindlessTitan>() != null))
        //        {
        //            this.ungrabbed();
        //            this.Rigidbody.velocity = (Vector3) (Vector3.up * 30f);
        //            base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
        //            if (PhotonNetwork.isMasterClient)
        //            {
        //                this.titanWhoGrabMe.GetComponent<MindlessTitan>().GrabEscapeRpc();
        //            }
        //            else
        //            {
        //                PhotonView.Find(this.titanWhoGrabMeID).RPC("GrabEscapeRpc", PhotonTargets.MasterClient, new object[0]);
        //            }
        //        }
        //    }

        public override bool Use()
        {
            if ((Hero.State != HumanState.Grabbed && Hero._state != HERO_STATE.Grab) || IsActive) return false;

            if (TimesUsed < TimesAllowed && !Hero.Animation.IsPlaying("grabbed_jean"))
            {
                Hero.PlayAnimation("grabbed_jean");
                TimesUsed++;
                IsActive = true;
                return true;
            }

            return false;
        }

        public override void OnUpdate()
        {
            if (Hero.Animation.IsPlaying("grabbed_jean") && Hero.Animation["grabbed_jean"].normalizedTime > 0.64f)
            {
                Hero.Ungrabbed();
                Hero.Rigidbody.velocity = Vector3.up * 30f;
                IsActive = false;
            }
            else
            {
                IsActive = true;
            }

        }

        //TODO: Execute skill logic once activated

    }
}
