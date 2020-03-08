﻿using System.Collections.Generic;
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
        Hero.crossFade(Hero.reloadAnimation, 0.1f);
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
            if (!((Hero.baseAnimation[Hero.reloadAnimation].normalizedTime < 0.13f) || Hero.throwedBlades))
            {
                Hero.throwedBlades = true;
                if (Hero.setup.part_blade_l.activeSelf)
                {
                    ThrowBlades();
                }
            }
            if ((Hero.baseAnimation[Hero.reloadAnimation].normalizedTime >= 0.37f) && (Hero.currentBladeNum > 0))
            {
                Hero.setup.part_blade_l.SetActive(true);
                Hero.setup.part_blade_r.SetActive(true);
                Hero.currentBladeSta = Hero.totalBladeSta;
            }
        }
    }

    private void ThrowBlades()
    {
        var transform = Hero.setup.part_blade_l.transform;
        var transform2 = Hero.setup.part_blade_r.transform;
        var obj2 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
        var obj3 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
        obj2.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
        obj3.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
        Vector3 force = (Hero.transform.forward + ((Vector3)(Hero.transform.up * 2f))) - Hero.transform.right;
        obj2.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        Vector3 vector2 = (Hero.transform.forward + ((Vector3)(Hero.transform.up * 2f))) + Hero.transform.right;
        obj3.GetComponent<Rigidbody>().AddForce(vector2, ForceMode.Impulse);
        Vector3 torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        torque.Normalize();
        obj2.GetComponent<Rigidbody>().AddTorque(torque);
        torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        torque.Normalize();
        obj3.GetComponent<Rigidbody>().AddTorque(torque);
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
}
