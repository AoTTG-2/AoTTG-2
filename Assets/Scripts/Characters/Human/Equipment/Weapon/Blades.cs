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
    public override void SetStats(HeroStat heroStat)
    {
        base.SetStats(heroStat);
        maxDurability = heroStat.BLA;
        BladeDurability = maxBlades;
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
