using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Body
{
    public class MindlessTitanBody : TitanBody
    {
        public Transform checkAeLLeft;
        public Transform checkAeLRight;
        public Transform checkAeRight;
        public Transform checkAeLeft;

        [Header("Grab Detection")]
        public Transform CheckOverhead;
        public Transform CheckFrontRight;
        public Transform CheckFrontLeft;
        public Transform CheckFront;
        public Transform CheckBackRight;
        public Transform CheckBackLowRight;
        public Transform CheckBackLowLeft;
        public Transform CheckBackLeft;
        public Transform CheckBack;

        [Header("Attack Detection")]
        public Transform AttackStomp;
        public Transform AttackSlapFace;
        public Transform AttackSlapBack;
        public Transform AttackKick;
        public Transform AttackFrontGround;
        public Transform AttackCombo;
        public Transform AttackBiteRight;
        public Transform AttackBiteLeft;
        public Transform AttackBite;
        public Transform AttackAbnormalJump;

    }
}
