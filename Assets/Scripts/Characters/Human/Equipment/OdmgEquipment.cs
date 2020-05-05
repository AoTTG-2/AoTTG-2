using UnityEngine;

public interface Weapon
{
    void Attack();
    void Reload();
}

public abstract class OdmgEquipment : MonoBehaviour
{
    protected Hero myHeroScript;

    [SerializeField] public float maxGas = 100f;
    public float currentGas = 100f;
    public float useGasSpeed = 0.2f;

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
    #endregion
}
