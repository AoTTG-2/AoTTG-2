using UnityEngine;

public enum EquipmentType
{
    Blades,
    AHSS,
    ThunderSpear
}

public interface Weapon
{
    void Attack();
    void PlayReloadAnimation();
}

public abstract class OdmgEquipment : MonoBehaviour
{
    protected GameObject heroObject;
    protected Hero myHeroScript;
    protected HERO_SETUP heroSetupScript;
    protected ArmatureData heroArmature;


    [SerializeField] public GameObject hookLaunchPointLeft;
    [SerializeField] public GameObject hookLaunchPointRight;

    public GameObject part_3dmg;
    public GameObject part_3dmg_belt;
    public GameObject part_3dmg_gas_l;
    public GameObject part_3dmg_gas_r;
    public GameObject part_blade_l;
    public GameObject part_blade_r;

    [SerializeField] public float maxGas = 100f;
    public float currentGas = 100f;
    public float useGasSpeed = 0.2f;
    public float dashTime;

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        Equip();
    }

    protected virtual void Update() { }

    public void SetHero(GameObject heroObject)
    {
        this.heroObject = heroObject;
        myHeroScript = heroObject.GetComponent<Hero>();
        heroSetupScript = heroObject.GetComponent<HERO_SETUP>();
        heroArmature = heroObject.GetComponent<ArmatureData>();
    }

    #region Virtual Methods
    public virtual void SetStats(HeroStat heroStat)
    {
        maxGas = heroStat.GAS;
        currentGas = maxGas;
    }

    public virtual void Equip()
    {
        if (heroSetupScript.myCostume.mesh_3dmg.Length > 0)
        {
            part_3dmg = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.mesh_3dmg));
            part_3dmg.transform.position = heroObject.transform.position;
            part_3dmg.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
            part_3dmg.transform.parent = heroArmature.chest;
            part_3dmg.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];
        }
        if (heroSetupScript.myCostume.mesh_3dmg_belt.Length > 0)
        {
            part_3dmg_belt = heroSetupScript.GenerateCloth(heroSetupScript.reference, "Character/" + heroSetupScript.myCostume.mesh_3dmg_belt);
            part_3dmg_belt.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];
        }
        if (heroSetupScript.myCostume.mesh_3dmg_gas_l.Length > 0)
        {
            part_3dmg_gas_l = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.mesh_3dmg_gas_l));
            if (heroSetupScript.myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
            {
                part_3dmg_gas_l.transform.position = heroObject.transform.position;
                part_3dmg_gas_l.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                part_3dmg_gas_l.transform.parent = heroArmature.spine;
            }
            else
            {
                part_3dmg_gas_l.transform.position = heroObject.transform.position;
                part_3dmg_gas_l.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                part_3dmg_gas_l.transform.parent = heroArmature.thigh_L;
            }
            part_3dmg_gas_l.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];
        }
        if (heroSetupScript.myCostume.mesh_3dmg_gas_r.Length > 0)
        {
            part_3dmg_gas_r = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.mesh_3dmg_gas_r));
            if (heroSetupScript.myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
            {
                part_3dmg_gas_r.transform.position = heroObject.transform.position;
                part_3dmg_gas_r.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                part_3dmg_gas_r.transform.parent = heroArmature.spine;
            }
            else
            {
                part_3dmg_gas_r.transform.position = heroObject.transform.position;
                part_3dmg_gas_r.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
                part_3dmg_gas_r.transform.parent = heroArmature.thigh_R;
            }
            part_3dmg_gas_r.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];
        }
        if (heroSetupScript.myCostume.weapon_l_mesh.Length > 0)
        {
            part_blade_l = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.weapon_l_mesh));
            part_blade_l.transform.position = heroObject.transform.position;
            part_blade_l.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
            part_blade_l.transform.parent = heroArmature.hand_L;
            part_blade_l.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];
        }
        if (heroSetupScript.myCostume.weapon_r_mesh.Length > 0)
        {
            part_blade_r = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character/" + heroSetupScript.myCostume.weapon_r_mesh));
            part_blade_r.transform.position = heroObject.transform.position;
            part_blade_r.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
            part_blade_r.transform.parent = heroArmature.hand_R;
            part_blade_r.GetComponent<Renderer>().material = CharacterMaterials.materials[heroSetupScript.myCostume._3dmg_texture];
        }
    }

    public virtual void Unequip()
    {
        Destroy(this.part_3dmg);
        Destroy(this.part_3dmg_belt);
        Destroy(this.part_3dmg_gas_l);
        Destroy(this.part_3dmg_gas_r);
        Destroy(this.part_blade_l);
        Destroy(this.part_blade_r);
    }

    public virtual bool NeedResupply()
    {
        if (currentGas != maxGas)
            return true;

        return false;
    }

    public virtual void Resupply()
    {
        currentGas = maxGas;
    }

    public virtual void updateSupplyUI()
    {
        float gasPercentage = currentGas / maxGas;
        Color gasIconColor;

        myHeroScript.cachedSprites["GasLeft"].fillAmount = myHeroScript.cachedSprites["GasRight"].fillAmount = gasPercentage;

        if (gasPercentage <= 0.25f)
            gasIconColor = Color.red;
        else if (gasPercentage < 0.5f)
            gasIconColor = Color.yellow;
        else
            gasIconColor = Color.white;

        myHeroScript.cachedSprites["GasLeft"].color = gasIconColor;
        myHeroScript.cachedSprites["GasRight"].color = gasIconColor;
    }
    #endregion

    #region ODMG Methods
    public void FillGas()
    {
        currentGas = maxGas;
    }

    //Consume gas for the current frame at the default rate
    public void UseGas()
    {
        UseGas(useGasSpeed * Time.deltaTime);
    }

    //Consume a set percentage of gas
    public void UseGas(float percentageUsed)
    {
        currentGas -= percentageUsed;

        if (currentGas < 0f)
            currentGas = 0f;
    }

    public void dash(float xAxisVeclocity, float zAxisVelocity)
    {
        print(dashTime + " " + currentGas);

        if (((dashTime <= 0f) && (currentGas > 0f)) && !myHeroScript.isMounted)
        {
            UseGas(maxGas * 0.04f);
            myHeroScript.facingDirection = myHeroScript.getGlobalFacingDirection(xAxisVeclocity, zAxisVelocity);
            myHeroScript.dashV = myHeroScript.getGlobaleFacingVector3(myHeroScript.facingDirection);
            myHeroScript.originVM = myHeroScript.currentSpeed;
            Quaternion quaternion = Quaternion.Euler(0f, myHeroScript.facingDirection, 0f);
            myHeroScript.GetComponent<Rigidbody>().rotation = quaternion;
            myHeroScript.targetRotation = quaternion;

            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                Instantiate(Resources.Load("FX/boost_smoke"), heroObject.transform.position, heroObject.transform.rotation);
            else
                PhotonNetwork.Instantiate("FX/boost_smoke", heroObject.transform.position, heroObject.transform.rotation, 0);

            dashTime = 0.5f;
            myHeroScript.crossFade("dash", 0.1f);
            myHeroScript.GetComponent<Animation>()["dash"].time = 0.1f;
            myHeroScript.state = HERO_STATE.AirDodge;
            myHeroScript.falseAttack();
            myHeroScript.GetComponent<Rigidbody>().AddForce((Vector3)(myHeroScript.dashV * 40f), ForceMode.VelocityChange);
        }
    }
    #endregion
}
