using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CombatController : MonoBehaviour
{
	public WeaponBase equipedWeapon = null;
	public float distance = 0.5f;

	// Events
	public UnityEvent onAttackEvent = new UnityEvent();
	public UnityEvent onChargeEvent = new UnityEvent();
	public UnityEvent<GameObject> onWeaponChangeEvent = new UnityEvent<GameObject>();

	private Rigidbody2D m_rigidbody;
	
	private bool m_attackInput = false;

	private Vector2 m_mousePos = Vector3.zero;

	public void OnAttack(InputAction.CallbackContext context) { if (m_attackInput != context.performed) Attack(context); }
	public void OnMousePosition(InputAction.CallbackContext context) { m_mousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()); }

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

	private void Update()
	{
		PositionAndRotateWeapon();
	}

	private void Attack(InputAction.CallbackContext context)
	{
		if (context.performed)
			onChargeEvent?.Invoke(); 
		else 
			onAttackEvent?.Invoke(); 
		
		m_attackInput = context.performed;
	}

	public void PositionAndRotateWeapon()
	{
		Vector2 mouseRelPos = m_mousePos - m_rigidbody.position;
		float angle = Mathf.Atan2(mouseRelPos.y, mouseRelPos.x);

		// Position
		Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		equipedWeapon.rigidbody.position = m_rigidbody.position + (offset * distance);

		// Rotation
		equipedWeapon.rigidbody.rotation = angle * Mathf.Rad2Deg - 90.0f;
	}
}
