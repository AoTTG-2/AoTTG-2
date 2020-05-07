using UnityEngine;

public class AHSS : OdmgEquipment, Weapon
{
    [SerializeField] public int maxAmmo = 7;
    public int leftGunAmmo { get; set; }
    public int rightGunAmmo { get; set; }
    public bool leftGunLoaded { get; set; }
    public bool rightGunLoaded { get; set; }

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
        if (myHeroScript.grounded || FengGameManagerMKII.Gamemode.AhssAirReload)
        {
            myHeroScript.state = HERO_STATE.ChangeBlade;
            myHeroScript.throwedBlades = false;

            if (!leftGunLoaded && !rightGunLoaded)
            {
                if (myHeroScript.grounded)
                {
                    myHeroScript.reloadAnimation = "AHSS_gun_reload_both";
                }
                else
                {
                    myHeroScript.reloadAnimation = "AHSS_gun_reload_both_air";
                }
            }
            else if (!leftGunLoaded)
            {
                if (myHeroScript.grounded)
                {
                    myHeroScript.reloadAnimation = "AHSS_gun_reload_l";
                }
                else
                {
                    myHeroScript.reloadAnimation = "AHSS_gun_reload_l_air";
                }
            }
            else if (!rightGunLoaded)
            {
                if (myHeroScript.grounded)
                {
                    myHeroScript.reloadAnimation = "AHSS_gun_reload_r";
                }
                else
                {
                    myHeroScript.reloadAnimation = "AHSS_gun_reload_r_air";
                }
            }

            myHeroScript.crossFade(myHeroScript.reloadAnimation, 0.05f);
        }
    }
    #endregion

    #region AHSS Methods
    #endregion
}