using UnityEngine;

public class AHSS : OdmgEquipment, Weapon
{
    [SerializeField] private GameObject myHero;
    private Hero myHeroScript;

    [SerializeField] private int maxAmmo = 7;
    public int leftGunAmmo { get; private set; }
    public int rightGunAmmo { get; private set; }

    protected override void Awake()
    {
        myHeroScript = myHero.GetComponent<Hero>();
        base.Initialize(myHeroScript);

        leftGunAmmo = maxAmmo;
        rightGunAmmo = maxAmmo;
    }

    protected override void Update()
    {
    }

    public void Attack()
    {

    }

    public void Reload()
    {

    }

    public new void Resupply()
    {
        base.Resupply();
    }
}