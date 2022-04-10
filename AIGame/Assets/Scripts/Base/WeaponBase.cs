using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class WeaponBase : MonoBehaviour
{
	public EntityBase owner;

	public float chargeTime;
	public float desiredRange = 0;

	private float currentChargeTime = 0;

	[HideInInspector]
	public new Rigidbody2D rigidbody;
	private CombatController combatController;

	protected virtual void Start()
	{
		if (rigidbody == null)
			rigidbody = GetComponent<Rigidbody2D>();

		if (owner != null)
			combatController = owner.GetComponent<CombatController>();
		
		if (combatController)
			combatController.onWeaponChangeEvent.AddListener(Swap);
	}

	protected virtual void OnDestroy()
	{
		combatController.onWeaponChangeEvent.RemoveListener(Swap);
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
		combatController.onAttackEvent.AddListener(Attack);
		combatController.onChargeEvent.AddListener(Charge);
	}

	public virtual void Unequip()
	{
		combatController.onAttackEvent.RemoveListener(Attack);
		combatController.onChargeEvent.RemoveListener(Charge);
	}

	public virtual void Attack(float charge)
    {
		currentChargeTime = 0;
    }

	public virtual void Charge(float charge)
    {
		currentChargeTime += Time.deltaTime;
		combatController.charge = Mathf.Clamp01(currentChargeTime / chargeTime);
    }
}