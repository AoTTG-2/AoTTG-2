using UnityEngine;

public class AHSS : OdmgEquipment, Weapon
{
    [SerializeField] private int maxAmmo = 7;
    public int leftGunAmmo { get; private set; }
    public int rightGunAmmo { get; private set; }

    protected override void Awake()
    {
        leftGunAmmo = maxAmmo;
        rightGunAmmo = maxAmmo;
    }

    protected override void Update()
    {
    }

    #region Equipment Methods
    public override void SetStats(HeroStat heroStat)
    {
        base.SetStats(heroStat);
    }

    public override bool NeedResupply()
    {
        if (base.NeedResupply() || leftGunAmmo != maxAmmo || rightGunAmmo != maxAmmo)
            return true;

        return false;
    }

    public override void Resupply()
    {
        base.Resupply();
        leftGunAmmo = maxAmmo;
        rightGunAmmo = maxAmmo;
    }
    #endregion

    #region Weapon Methods
    public void Attack()
    {

    }

    public void Reload()
    {

    }
    #endregion
}