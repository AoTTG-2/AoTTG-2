using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    public class TitanBody : MonoBehaviour
    {
        public Transform Head;
        public Transform Neck;
        public Transform HandRight;
        public Transform HandLeft;
        public Transform Chest;
        public Transform ArmRight;
        public Transform UpperArmRight;
        public Transform ShoulderRight;

        public Transform ArmLeft;
        public Transform UpperArmLeft;
        public Transform ShoulderLeft;

        public Transform Hip;

        public Transform LegLeft;
        public Transform LegRight;

        public Transform checkAeLLeft;
        public Transform checkAeLRight;
        public Transform checkAeRight;
        public Transform checkAeLeft;

        public Transform CheckOverhead;
        public Transform CheckFrontRight;
        public Transform CheckFrontLeft;
        public Transform CheckFront;
        public Transform CheckBackRight;
        public Transform CheckBackLowRight;
        public Transform CheckBackLowLeft;
        public Transform CheckBackLeft;
        public Transform CheckBack;

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

        private Dictionary<BodyPart, float> CooldownDictionary { get; set; } = new Dictionary<BodyPart, float>();
        private readonly float _bodyPartRecovery = 45f;
        private readonly Vector3 bodyPartDamagedSize = new Vector3(0.001f, 0.001f, 0.001f);

        public void AddBodyPart(Transform body)
        {
            Debug.Log($"We've hit the {body.gameObject.name}");
            BodyPart[] bodyPart = {BodyPart.None};
            Transform bodyPartEffect = null;
            if (body == UpperArmRight || body == ArmRight)
            {
                bodyPart = new[] {BodyPart.ArmRight, BodyPart.HandRight};
                bodyPartEffect = ArmRight;
            } else if (body == UpperArmLeft || body == ArmLeft)
            {
                bodyPart = new[] { BodyPart.ArmLeft, BodyPart.HandLeft };
                bodyPartEffect = ArmLeft;
            } else if (body == LegLeft)
            {
                bodyPart = new[] {BodyPart.LegLeft};
                bodyPartEffect = LegLeft;
            } else if (body == LegRight)
            {
                bodyPart = new[] { BodyPart.LegRight };
                bodyPartEffect = LegRight;
            }

            if (bodyPart[0] == BodyPart.None || CooldownDictionary.ContainsKey(bodyPart[0])) return;

            foreach (var part in bodyPart)
            {
                CooldownDictionary.Add(part, _bodyPartRecovery);
            }

            if (bodyPartEffect == null) return;
            var steamEffect = PhotonNetwork.Instantiate("fx/bodypart_steam", new Vector3(), new Quaternion(), 0);
            steamEffect.GetComponent<SelfDestroy>().CountDown = _bodyPartRecovery;
            steamEffect.transform.parent = bodyPartEffect;
            steamEffect.transform.localPosition = new Vector3();
        }

        public List<BodyPart> GetDisabledBodyParts()
        {
            return new List<BodyPart>(CooldownDictionary.Keys);
        }

        private void SetDamagedBodyPart(BodyPart bodyPart)
        {
            switch (bodyPart)
            {
                case BodyPart.ArmRight:
                    ArmRight.localScale = bodyPartDamagedSize;
                    break;
                case BodyPart.ArmLeft:
                    ArmLeft.localScale = bodyPartDamagedSize;
                    break;
                case BodyPart.LegRight:
                    LegRight.localScale = bodyPartDamagedSize;
                    break;
                case BodyPart.LegLeft:
                    LegLeft.localScale = bodyPartDamagedSize;
                    break;
            }
        }

        private void LateUpdate()
        {
            foreach (var bodyPart in new List<BodyPart>(CooldownDictionary.Keys))
            {
                CooldownDictionary[bodyPart] -= Time.deltaTime;
                if (CooldownDictionary[bodyPart] > 0)
                {
                    SetDamagedBodyPart(bodyPart);
                }
                else
                {
                    CooldownDictionary.Remove(bodyPart);
                }
            }
        }
    }
}
