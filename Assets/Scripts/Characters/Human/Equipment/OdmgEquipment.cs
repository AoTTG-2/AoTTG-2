using UnityEngine;

public interface Weapon
{
    void Attack();
    void Reload();
}

public abstract class OdmgEquipment : MonoBehaviour
{
    protected Hero myHeroScript;

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }

    public void SetHero(Hero myHeroScript)
    {
        this.myHeroScript = myHeroScript;
    }

    public void FillGass()
    {

    }
}
