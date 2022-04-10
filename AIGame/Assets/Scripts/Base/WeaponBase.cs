using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class WeaponBase : MonoBehaviour
{
    [HideInInspector]
    public new Rigidbody2D rigidbody;
    private static CombatController CombatController;

    protected virtual void Start()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();

        if (CombatController == null)
            CombatController = FindObjectOfType<CombatController>();

        CombatController.onWeaponChangeEvent.AddListener(Swap);
    }

    protected virtual void OnDestroy()
    {
        CombatController.onWeaponChangeEvent.RemoveListener(Swap);
    }

    private void Swap(GameObject weapon)
    {
        if (weapon == gameObject)
        {
            Equip();
            return;
        }

        Unequip();
    }

    public virtual void Equip()
    {
        CombatController.onAttackEvent.AddListener(Attack);
        CombatController.onChargeEvent.AddListener(Charge);
    }

    public virtual void Unequip()
    {
        CombatController.onAttackEvent.RemoveListener(Attack);
        CombatController.onChargeEvent.RemoveListener(Charge);
    }

    public abstract void Attack();
    public abstract void Charge();
}