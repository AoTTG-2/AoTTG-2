using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    public class TitanBody : Photon.MonoBehaviour
    {
        public Transform AABB;
        public Transform Core;

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

        private float LimbRegeneration { get; set; }
        private float LimbHealth { get; set; }
        private readonly Vector3 bodyPartDamagedSize = new Vector3(0.001f, 0.001f, 0.001f);
        private Dictionary<BodyPart, GameObject> SteamEffectDictionary { get; set; } = new Dictionary<BodyPart, GameObject>();
        private Dictionary<BodyPart, float> CooldownDictionary { get; set; } = new Dictionary<BodyPart, float>();
        private Dictionary<BodyPart, float> HealthDictionary { get; set; }

        public void Initialize(float limbHealth, float limbRegeneration)
        {
            LimbHealth = limbHealth;
            LimbRegeneration = limbRegeneration;
            HealthDictionary = new Dictionary<BodyPart, float>();
            foreach (BodyPart bodyPart in Enum.GetValues(typeof(BodyPart)))
            {
                HealthDictionary[bodyPart] = LimbHealth;
            }
        }

        public BodyPart GetBodyPart(Transform bodypart)
        {
            if (bodypart == UpperArmRight || bodypart == ArmRight)
            {
                return BodyPart.ArmRight;
            }
            if (bodypart == UpperArmLeft || bodypart == ArmLeft)
            {
                return BodyPart.ArmLeft;
            }
            if (bodypart == LegLeft)
            {
                return BodyPart.LegLeft;
            }
            if (bodypart == LegRight)
            {
                return BodyPart.LegRight;
            }

            return BodyPart.None;
        }

        public void DamageBodyPart(BodyPart body, int damage)
        {
            HealthDictionary[body] -= damage;
            if (HealthDictionary[body] <= 0)
            {
                AddBodyPart(body);
                HealthDictionary[body] = LimbHealth;
            }
        }

        public void AddBodyPart(BodyPart body)
        {
            BodyPart[] bodyPart = {BodyPart.None};
            Transform bodyPartEffect = null;
            if (body == BodyPart.ArmRight)
            {
                bodyPart = new[] {BodyPart.ArmRight, BodyPart.HandRight};
                bodyPartEffect = ArmRight;
            } else if (body == BodyPart.ArmLeft)
            {
                bodyPart = new[] { BodyPart.ArmLeft, BodyPart.HandLeft };
                bodyPartEffect = ArmLeft;
            } else if (body == BodyPart.LegLeft)
            {
                bodyPart = new[] {BodyPart.LegLeft};
                bodyPartEffect = LegLeft;
            } else if (body == BodyPart.LegRight)
            {
                bodyPart = new[] { BodyPart.LegRight };
                bodyPartEffect = LegRight;
            }

            if (bodyPart[0] == BodyPart.None || CooldownDictionary.ContainsKey(bodyPart[0])) return;

            foreach (var part in bodyPart)
            {
                CooldownDictionary.Add(part, LimbRegeneration);
            }

            if (bodyPartEffect == null || !photonView.isMine) return;
            var steamEffect = PhotonNetwork.Instantiate("fx/bodypart_steam", new Vector3(), new Quaternion(), 0);
            steamEffect.GetComponent<SelfDestroy>().CountDown = LimbRegeneration;
            steamEffect.transform.parent = bodyPartEffect;
            steamEffect.transform.localPosition = new Vector3();
            SteamEffectDictionary.Add(body, steamEffect);
            photonView.RPC("SyncBodyPartRpc", PhotonTargets.Others, bodyPart);
        }

        [PunRPC]
        private void SyncBodyPartRpc(BodyPart[] parts, PhotonMessageInfo info = new PhotonMessageInfo())
        {
            if (info.sender.ID != photonView.owner.ID) return;

            foreach (var part in parts)
            {
                AddBodyPart(part);
            }
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
                    SteamEffectDictionary.Remove(bodyPart);
                }
            }
        }

        void OnDestroy()
        {
            if (!photonView.isMine) return;
            foreach (var effect in SteamEffectDictionary)
            {
                if (effect.Value != null)
                {
                    PhotonNetwork.Destroy(effect.Value);
                }
            }
        }
    }
}
