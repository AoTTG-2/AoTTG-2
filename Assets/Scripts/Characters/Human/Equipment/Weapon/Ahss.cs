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

    public void Resupply()
    {

    }

    #region Weapon Methods
    public void Attack()
    {

    }

    public void Reload()
    {

    }
    #endregion
}