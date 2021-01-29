using System;
using System.Collections.Generic;
using System.Linq;
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

        private float LimbRegeneration { get; set; }
        private float DefaultLimbHealth { get; set; }
        private readonly Vector3 bodyPartDamagedSize = new Vector3(0.001f, 0.001f, 0.001f);
        private Dictionary<BodyPart, GameObject> SteamEffectDictionary { get; set; } = new Dictionary<BodyPart, GameObject>();
        private Dictionary<BodyPart, float> CooldownDictionary { get; set; } = new Dictionary<BodyPart, float>();
        private Dictionary<BodyPart, float> HealthDictionary { get; set; }

        public void Initialize(float defaultLimbHealth, float limbRegeneration)
        {
            DefaultLimbHealth = defaultLimbHealth;
            LimbRegeneration = limbRegeneration;
            HealthDictionary = new Dictionary<BodyPart, float>();
            foreach (BodyPart bodyPart in Enum.GetValues(typeof(BodyPart)))
            {
                HealthDictionary[bodyPart] = DefaultLimbHealth;
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

        public void DamageBodyPart(BodyPart bodyPart, int damage)
        {
            DamageBodyPart(bodyPart, damage, LimbRegeneration);
        }

        public void DamageBodyPart(BodyPart bodyPart, int damage, float regenerationTime)
        {
            HealthDictionary[bodyPart] -= damage;
            if (HealthDictionary[bodyPart] <= 0)
            {
                AddDamagedBodyPart(bodyPart, regenerationTime);
                HealthDictionary[bodyPart] = DefaultLimbHealth;
            }
        }

        public void AddDamagedBodyPart(BodyPart bodyPart)
        {
            AddDamagedBodyPart(bodyPart, LimbRegeneration);
        }

        public void AddDamagedBodyPart(BodyPart bodyPart, float regenerationTime)
        {
            var damagedBodyParts = new List<BodyPart>();
            Transform bodyPartEffect = null;

            switch (bodyPart)
            {
                case BodyPart.ArmRight:
                    damagedBodyParts.Add(BodyPart.ArmRight);
                    damagedBodyParts.Add(BodyPart.HandRight);
                    bodyPartEffect = ArmRight;
                    break;
                case BodyPart.ArmLeft:
                    damagedBodyParts.Add(BodyPart.ArmLeft);
                    damagedBodyParts.Add(BodyPart.HandLeft);
                    bodyPartEffect = ArmLeft;
                    break;
                case BodyPart.LegLeft:
                    damagedBodyParts.Add(BodyPart.LegLeft);
                    bodyPartEffect = LegLeft;
                    break;
                case BodyPart.LegRight:
                    damagedBodyParts.Add(BodyPart.LegRight);
                    bodyPartEffect = LegRight;
                    break;
                case BodyPart.Eyes:
                    damagedBodyParts.Add(BodyPart.Eyes);
                    break;
                case BodyPart.Ankle:
                    damagedBodyParts.Add(BodyPart.Ankle);
                    break;
                default:
                    return;
            }

            foreach (var part in damagedBodyParts)
            {
                if (CooldownDictionary.ContainsKey(part)) return;
            }

            foreach (var part in damagedBodyParts)
            {
                CooldownDictionary.Add(part, regenerationTime);
            }

            if (bodyPartEffect != null && photonView.isMine)
            {
                var steamEffect = PhotonNetwork.Instantiate("fx/bodypart_steam", new Vector3(), new Quaternion(), 0);
                steamEffect.GetComponent<SelfDestroy>().CountDown = regenerationTime;
                steamEffect.transform.parent = bodyPartEffect;
                steamEffect.transform.localPosition = new Vector3();
                SteamEffectDictionary.Add(bodyPart, steamEffect);
                photonView.RPC(nameof(SyncDamagedBodyPartRpc), PhotonTargets.Others, damagedBodyParts);
            }
        }

        [PunRPC]
        protected void SyncDamagedBodyPartRpc(BodyPart[] parts, PhotonMessageInfo info)
        {
            if (info.sender.ID != photonView.owner.ID) return;
            foreach (var part in parts)
            {
                AddDamagedBodyPart(part);
            }
        }

        public Dictionary<BodyPart,float>.KeyCollection GetDisabledBodyParts()
        {
            return CooldownDictionary.Keys;
        }

        public bool IsDisabled(params BodyPart[] bodyParts)
        {
            if (bodyParts == null) return false;
            return bodyParts.All(bodyPart => GetDisabledBodyParts().Contains(bodyPart));
        }

        private void SetDamagedBodyPartSize(BodyPart bodyPart)
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
            foreach (var bodyPart in CooldownDictionary.Keys.ToArray())
            {
                CooldownDictionary[bodyPart] -= Time.deltaTime;
                if (CooldownDictionary[bodyPart] > 0)
                {
                    SetDamagedBodyPartSize(bodyPart);
                }
                else
                {
                    CooldownDictionary.Remove(bodyPart);
                    SteamEffectDictionary.Remove(bodyPart);
                }
            }
        }

        private void OnDestroy()
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
