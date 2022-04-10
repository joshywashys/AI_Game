using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
	public WeaponBase equipedWeapon = null;
	public float distance = 0.5f;
	public float charge = 0.0f;

	// Events
	public UnityEvent<float> onAttackEvent = new UnityEvent<float>();
	public UnityEvent<float> onChargeEvent = new UnityEvent<float>();
	public UnityEvent<GameObject> onWeaponChangeEvent = new UnityEvent<GameObject>();

	private Rigidbody2D m_rigidbody;

	// Setters
	public void EquipWeapon(WeaponBase weapon) { equipedWeapon = weapon; onWeaponChangeEvent?.Invoke(weapon.gameObject); }

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

    public void Start()
    {
		equipedWeapon.Equip();
	}

	public void Attack()
	{
		if (isCharging != null)
		{
			StopCoroutine(isCharging);
			isCharging = null;
		}
		onAttackEvent?.Invoke(charge); 	
	}

	public void Charge()
    {
		if (isCharging == null)
			isCharging = StartCoroutine(Charging());
	}

	private Coroutine isCharging = null;
	private IEnumerator Charging()
    {
		while (true)
		{
			onChargeEvent?.Invoke(charge);
			yield return null;
		}
	}

	public void Aim(Vector2 target)
	{
		Vector2 aimRelPos = target - m_rigidbody.position;
		float angle = Mathf.Atan2(aimRelPos.y, aimRelPos.x);

		// Position
		Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		equipedWeapon.rigidbody.position = m_rigidbody.position + (offset * distance);

		// Rotation
		equipedWeapon.rigidbody.rotation = angle * Mathf.Rad2Deg - 90.0f;
	}
}
