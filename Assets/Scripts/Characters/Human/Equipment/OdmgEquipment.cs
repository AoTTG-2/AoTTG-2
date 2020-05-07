using UnityEngine;

public interface Weapon
{
    void Attack();
    void PlayReloadAnimation();
}

public abstract class OdmgEquipment : MonoBehaviour
{
    protected Hero myHeroScript;

    [SerializeField] public GameObject hookLaunchPointLeft;
    [SerializeField] public GameObject hookLaunchPointRight;

    [SerializeField] public float maxGas = 100f;
    public float currentGas = 100f;
    public float useGasSpeed = 0.2f;
    public float dashTime;

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }

    public void SetHero(Hero myHeroScript)
    {
        this.myHeroScript = myHeroScript;
    }

    #region Virtual Methods
    public virtual void SetStats(HeroStat heroStat)
    {
        this.maxGas = heroStat.GAS;
        currentGas = maxGas;
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
                Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
            else
                PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);

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
