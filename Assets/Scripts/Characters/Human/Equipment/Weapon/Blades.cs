using UnityEngine;

public class Blades : OdmgEquipment, Weapon
{
    [SerializeField] public int maxBlades = 5;
    [SerializeField] public float maxDurability = 100f;
    public int NumBlades { get; set; }
    public float BladeDurability { get; set; }

    protected override void Awake()
    {
        NumBlades = maxBlades;
        BladeDurability = maxDurability;
    }

    protected override void Update()
    {
    }

    #region Equipment Methods
    //public override void SetStats(HeroStat heroStat)
    //{
    //    base.SetStats(heroStat);
    //    maxDurability = heroStat.BLA;
    //    BladeDurability = maxDurability;
    //}

    public override void Equip()
    {
        base.Equip();

        //part_3dmg_belt = heroSetupScript.GenerateCloth(heroSetupScript.reference, "Character/" + heroSetupScript.myCostume.mesh_3dmg_belt);
        //part_3dmg_belt.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];

        part_3dmg_gas_l = Instantiate(prefab_3dmg_gas_l);
        part_3dmg_gas_l.transform.position = heroObject.transform.position;
        part_3dmg_gas_l.transform.parent = heroArmature.spine;
        
        part_3dmg_gas_r = Instantiate(prefab_3dmg_gas_r);
        part_3dmg_gas_r.transform.position = heroObject.transform.position;
        part_3dmg_gas_r.transform.parent = heroArmature.spine;

        //part_weapon_l = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.weapon_l_mesh));
        //part_weapon_l.transform.position = heroObject.transform.position;
        //part_weapon_l.transform.parent = heroArmature.hand_L;
        //part_weapon_l.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];

        //part_weapon_r = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.weapon_r_mesh));
        //part_weapon_r.transform.position = heroObject.transform.position;
        //part_weapon_r.transform.parent = heroArmature.hand_R;
        //part_weapon_r.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];

        part_weapon_l = Instantiate(prefab_weapon_l);
        part_weapon_l.transform.position = heroObject.transform.position;
        part_weapon_l.transform.parent = heroArmature.hand_L;

        part_weapon_r = Instantiate(prefab_weapon_r);
        part_weapon_r.transform.position = heroObject.transform.position;
        part_weapon_r.transform.parent = heroArmature.hand_R;
    }

    public override bool NeedResupply()
    {
        if (base.NeedResupply() || NumBlades != maxBlades || BladeDurability != maxDurability)
            return true;

        return false;
    }

    public override void Resupply()
    {
        base.Resupply();
        NumBlades = maxBlades;
        BladeDurability = maxDurability;
    }

    public override void updateSupplyUI()
    {
        base.updateSupplyUI();

        float durabilityPercentage = BladeDurability / maxDurability;
        Color bladesIconColor;

        var ammoUI = myHeroScript.InGameUI.GetComponentInChildren<Assets.Scripts.UI.InGame.Weapon.Blades>();
        ammoUI.SetBlades(NumBlades);

        //myHeroScript.cachedSprites["bladeCL"].fillAmount = durabilityPercentage;
        //myHeroScript.cachedSprites["bladeCR"].fillAmount = durabilityPercentage;

        //if (durabilityPercentage <= 0f)
        //    bladesIconColor = Color.red;
        //else if (durabilityPercentage < 0.3f)
        //    bladesIconColor = Color.yellow;
        //else
        //    bladesIconColor = Color.white;

        //myHeroScript.cachedSprites["bladel1"].color = bladesIconColor;
        //myHeroScript.cachedSprites["blader1"].color = bladesIconColor;
    }
    #endregion

    #region Weapon Methods
    public void Attack()
    {

    }

    public void PlayReloadAnimation()
    {
        myHeroScript.state = HERO_STATE.ChangeBlade;
        myHeroScript.throwedBlades = false;

        if (!myHeroScript.grounded)
            myHeroScript.reloadAnimation = "changeBlade_air";
        else
            myHeroScript.reloadAnimation = "changeBlade";

        myHeroScript.crossFade(myHeroScript.reloadAnimation, 0.1f);
    }
    #endregion

    #region Blades Methods
    public void useBlade(int amount = 0)
    {
        if (amount == 0)
            amount = 1;

        amount *= 2;

        if (BladeDurability > 0f)
        {
            BladeDurability -= amount;

            if (BladeDurability <= 0f)
            {
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || myHeroScript.photonView.isMine)
                {
                    myHeroScript.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                    myHeroScript.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                }
                BladeDurability = 0f;
                dropBlades();
            }
        }
    }

    public void dropBlades()
    {
        Transform transform = part_weapon_l.transform;
        Transform transform2 = part_weapon_r.transform;
        GameObject obj2 = (GameObject)Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
        GameObject obj3 = (GameObject)Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
        //obj2.GetComponent<Renderer>().material = CharacterMaterials.materials[setupScript.myCostume._3dmg_texture];
        //obj3.GetComponent<Renderer>().material = CharacterMaterials.materials[setupScript.myCostume._3dmg_texture];
        Vector3 force = (base.transform.forward + ((base.transform.up * 2f))) - base.transform.right;
        obj2.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        Vector3 vector2 = (base.transform.forward + ((base.transform.up * 2f))) + base.transform.right;
        obj3.GetComponent<Rigidbody>().AddForce(vector2, ForceMode.Impulse);
        Vector3 torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        torque.Normalize();
        obj2.GetComponent<Rigidbody>().AddTorque(torque);
        torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        torque.Normalize();
        obj3.GetComponent<Rigidbody>().AddTorque(torque);
        part_weapon_l.SetActive(false);
        part_weapon_r.SetActive(false);

        NumBlades--;

        if (NumBlades == 0)
            BladeDurability = 0f;

        if (myHeroScript.state == HERO_STATE.Attack)
            myHeroScript.falseAttack();
    }
    #endregion
}
