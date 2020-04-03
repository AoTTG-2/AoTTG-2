using UnityEngine;

public class Ahss : Weapon 
{
    public Ahss()
    {
        HookForwardLeft = "AHSS_hook_forward_l";
        HookForwardRight = "AHSS_hook_forward_r";
        HookForward = "AHSS_hook_forward_both";
        AmountLeft = AmountRight = 7;
    }

    public override void PlayReloadAnimation()
    {
        if (!Hero.leftGunHasBullet && !Hero.rightGunHasBullet)
        {
            if (Hero.grounded)
            {
                Hero.reloadAnimation = "AHSS_gun_reload_both";
            }
            else
            {
                Hero.reloadAnimation = "AHSS_gun_reload_both_air";
            }
        }
        else if (!Hero.leftGunHasBullet)
        {
            if (Hero.grounded)
            {
                Hero.reloadAnimation = "AHSS_gun_reload_l";
            }
            else
            {
                Hero.reloadAnimation = "AHSS_gun_reload_l_air";
            }
        }
        else if (!Hero.rightGunHasBullet)
        {
            if (Hero.grounded)
            {
                Hero.reloadAnimation = "AHSS_gun_reload_r";
            }
            else
            {
                Hero.reloadAnimation = "AHSS_gun_reload_r_air";
            }
        }
        else
        {
            if (Hero.grounded)
            {
                Hero.reloadAnimation = "AHSS_gun_reload_both";
            }
            else
            {
                Hero.reloadAnimation = "AHSS_gun_reload_both_air";
            }
            Hero.rightGunHasBullet = false;
            Hero.leftGunHasBullet = false;
        }
        Hero.crossFade(Hero.reloadAnimation, 0.05f);
    }

    public override void Reload()
    {
        if (Hero.baseAnimation[Hero.reloadAnimation].normalizedTime > 0.22f)
        {
            if (!(Hero.leftGunHasBullet || !Hero.setup.part_blade_l.activeSelf))
            {
                Hero.setup.part_blade_l.SetActive(false);
                Transform transform = Hero.setup.part_blade_l.transform;
                GameObject obj5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
                obj5.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
                Vector3 force = ((Vector3)((-Hero.baseTransform.forward * 10f) + (Hero.baseTransform.up * 5f))) - Hero.baseTransform.right;
                obj5.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                Vector3 torque = new Vector3((float)UnityEngine.Random.Range(-100, 100), (float)UnityEngine.Random.Range(-100, 100), (float)UnityEngine.Random.Range(-100, 100));
                obj5.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.Acceleration);
            }
            if (!(Hero.rightGunHasBullet || !Hero.setup.part_blade_r.activeSelf))
            {
                Hero.setup.part_blade_r.SetActive(false);
                Transform transform5 = Hero.setup.part_blade_r.transform;
                GameObject obj6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
                obj6.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
                Vector3 vector3 = ((Vector3)((-Hero.baseTransform.forward * 10f) + (Hero.baseTransform.up * 5f))) + Hero.baseTransform.right;
                obj6.GetComponent<Rigidbody>().AddForce(vector3, ForceMode.Impulse);
                Vector3 vector4 = new Vector3((float)UnityEngine.Random.Range(-300, 300), (float)UnityEngine.Random.Range(-300, 300), (float)UnityEngine.Random.Range(-300, 300));
                obj6.GetComponent<Rigidbody>().AddTorque(vector4, ForceMode.Acceleration);
            }
        }
        if ((Hero.baseAnimation[Hero.reloadAnimation].normalizedTime > 0.62f) && !Hero.throwedBlades)
        {
            Hero.throwedBlades = true;
            if (!((Hero.leftBulletLeft <= 0) || Hero.leftGunHasBullet))
            {
                AmountLeft--;
                Hero.leftBulletLeft--;
                Hero.setup.part_blade_l.SetActive(true);
                Hero.leftGunHasBullet = true;
            }
            if (!((Hero.rightBulletLeft <= 0) || Hero.rightGunHasBullet))
            {
                Hero.setup.part_blade_r.SetActive(true);
                AmountRight--;
                Hero.rightBulletLeft--;
                Hero.rightGunHasBullet = true;
            }
        }
    }

    public override void UpdateSupplyUi(GameObject inGameUi)
    {
        var bladesUi = inGameUi.GetComponentInChildren<Assets.Scripts.UI.InGame.Weapon.AHSS>();
        bladesUi.SetAHSS(AmountLeft, AmountRight);
    }
}
