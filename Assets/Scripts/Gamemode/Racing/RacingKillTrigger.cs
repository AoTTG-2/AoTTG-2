using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Constants;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using System;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingKillTrigger : MonoBehaviour
    {
        private Color OriginalColor { get; set; }
        private Material Material { get; set; }
        private Collider Collider { get; set; }

        private void Start()
        {
            Setting.Gamemode.WorldMode.OnValueChanged += WorldMode_OnValueChanged;
            Material = GetComponent<Renderer>()?.material;
            Collider = GetComponent<Collider>();

            if (Collider == null)
            {
                Destroy(this);
                throw new ArgumentNullException(nameof(Collider),
                    $"{gameObject.name} does not have a collider whereas this is required for RacingKillTrigger.");
            }

            if (Material != null)
            {
                OriginalColor = Material.color;
            }
        }

        private void OnDestroy()
        {
            Setting.Gamemode.WorldMode.OnValueChanged -= WorldMode_OnValueChanged;
        }

        private void WorldMode_OnValueChanged(WorldMode mode)
        {
            switch (mode)
            {
                case WorldMode.Default:
                case WorldMode.Lava:
                    ToggleLava();
                    break;
                case WorldMode.Water:
                    ToggleWater();
                    break;
                default:
                    ToggleLava();
                    break;
            }
        }

        private void ToggleLava()
        {
            Collider.isTrigger = true;
            if (Material != null) Material.color = OriginalColor;
        }

        private void ToggleWater()
        {
            Collider.isTrigger = false;
            if (Material != null)
            {
                Material.color = new Color(1.0f - OriginalColor.r, 1.0f - OriginalColor.g, 1.0f - OriginalColor.b, OriginalColor.a);
            }
        }

        
        private void OnTriggerEnter(Collider other)
        {
            CheckHero(other.gameObject);
        }

        private void CheckHero(GameObject hero)
        {
            if (hero.layer != (int) Layers.Player) return;
            hero = hero.transform.root.gameObject;
            if (hero.GetPhotonView() != null && hero.GetPhotonView().isMine)
            {
                var component = hero.GetComponent<Hero>();
                if (component != null)
                {
                    component.MarkDie();
                    component.photonView.RPC(nameof(Hero.NetDie2), PhotonTargets.All, -1, "Server");
                }
            }
        }
    }
}

