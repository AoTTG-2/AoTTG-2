using UnityEngine;

public class Blades : OdmgEquipment, Weapon
{
    [SerializeField] private int maxBlades = 5;
    [SerializeField] private float maxDurability = 100f;
    public int NumBlades { get; private set; }
    public float BladeDurability { get; private set; }

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
        maxBlades = heroStat.BLA;
        NumBlades = maxBlades;
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
