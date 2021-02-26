using Assets.Scripts.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Blades : Weapon 
{
    public Blades()
    {
        HookForwardLeft = "air_hook_l";
        HookForwardRight = "air_hook_r";
        HookForward = "air_hook";
        AmountLeft = AmountRight = 5;
    }

    public override void PlayReloadAnimation()
    {
        Hero.reloadAnimation = Hero.grounded
            ? "changeBlade"
            : "changeBlade_air";
        Hero.CrossFade(Hero.reloadAnimation, 0.1f);
    }

    public override void Reload()
    {
        if (!Hero.grounded)
        {
            if (!((Hero.GetComponent<Animation>()[Hero.reloadAnimation].normalizedTime < 0.2f) || Hero.throwedBlades))
            {
                Hero.throwedBlades = true;
                if (Hero.setup.part_blade_l.activeSelf)
                {
                    ThrowBlades();
                }
            }
            if ((Hero.GetComponent<Animation>()[Hero.reloadAnimation].normalizedTime >= 0.56f) && (Hero.currentBladeNum > 0))
            {
                Hero.setup.part_blade_l.SetActive(true);
                Hero.setup.part_blade_r.SetActive(true);
                Hero.currentBladeSta = Hero.totalBladeSta;
            }
        }
        else
        {
            if (!((Hero.animation[Hero.reloadAnimation].normalizedTime < 0.13f) || Hero.throwedBlades))
            {
                Hero.throwedBlades = true;
                if (Hero.setup.part_blade_l.activeSelf)
                {
                    ThrowBlades();
                }
            }
            if ((Hero.animation[Hero.reloadAnimation].normalizedTime >= 0.37f) && (Hero.currentBladeNum > 0))
            {
                Hero.setup.part_blade_l.SetActive(true);
                Hero.setup.part_blade_r.SetActive(true);
                Hero.currentBladeSta = Hero.totalBladeSta;
            }
        }
    }

    public override void UpdateSupplyUi(GameObject inGameUi)
    {
        var bladesUi = inGameUi.GetComponentInChildren<Assets.Scripts.UI.InGame.Weapon.Blades>();
        bladesUi.SetBlades(AmountLeft);
    }

	private void ThrowBlades()
    {
        var bladeLTransform = Hero.setup.part_blade_l.transform;
        var bladeRTransform = Hero.setup.part_blade_r.transform;
        var bladeL = Object.Instantiate(Resources.Load<GameObject>("Character_parts/character_blade_l"), bladeLTransform.position, bladeLTransform.rotation);
        var bladeR = Object.Instantiate(Resources.Load<GameObject>("Character_parts/character_blade_r"), bladeRTransform.position, bladeRTransform.rotation);

        bladeL.GetComponent<Renderer>(out var rendererL).GetComponent<Rigidbody>(out var rigidL);
        bladeR.GetComponent<Renderer>(out var rendererR).GetComponent<Rigidbody>(out var rigidR);

        var mat = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
        rendererL.material = mat;
        rendererR.material = mat;

        var fwd = (Hero.transform.forward + ((Hero.transform.up * 2f)));

        Vector3 force = fwd - Hero.transform.right;
        rigidL.AddForce(force, ForceMode.Impulse);
        force = fwd + Hero.transform.right;
        rigidR.AddForce(force, ForceMode.Impulse);
        Vector3 torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        rigidL.AddTorque(torque.normalized);
        torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        rigidR.AddTorque(torque.normalized);
        Hero.setup.part_blade_l.SetActive(false);
        Hero.setup.part_blade_r.SetActive(false);

        AmountLeft--;
        AmountRight--;
        Hero.currentBladeNum--;
        if (Hero.currentBladeNum == 0)
        {
            Hero.currentBladeSta = 0f;
        }
    }

    public override void Use(int amount = 0)
    {
        if (amount == 0)
        {
            amount = 1;
        }
        amount *= 2;
        if (Hero.currentBladeSta > 0f)
        {
            Hero.currentBladeSta -= amount;
            if (Hero.currentBladeSta <= 0f)
            {
                if (Hero.photonView.isMine)
                {
                    //this.leftbladetrail.Deactivate();
                    //this.rightbladetrail.Deactivate();
                    //this.leftbladetrail2.Deactivate();
                    //this.rightbladetrail2.Deactivate();
                    Hero.ActivateWeaponCollider(false, false);
                }
                Hero.currentBladeSta = 0f;
                ThrowBlades();
            }
        }
    }
}