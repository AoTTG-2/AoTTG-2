using UnityEngine;

public interface Weapon
{
    void Attack();
    void Reload();
}

public abstract class OdmgEquipment : MonoBehaviour
{
    protected Hero MyHero { get; private set; }

    protected void Initialize(Hero myHero)
    {
        MyHero = myHero;
    }

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }

    protected virtual void Resupply()
    {

    }
}
