using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityBase
{
	public CombatController CombatController { get; private set; } = null;
	public MovementController MovementController { get; private set; } = null;

	private bool m_attackInput = false;
	private Vector2 m_mousePos = Vector3.zero;

	public void OnAttack(InputAction.CallbackContext context) { if (m_attackInput != context.performed) Attack(context); }
	public void OnMousePosition(InputAction.CallbackContext context) { m_mousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()); }

	private void Attack(InputAction.CallbackContext context)
	{
		if (context.performed)
			CombatController?.Charge();
		else
			CombatController?.Attack();

		m_attackInput = context.performed;
	}

	private void Awake()
	{
		CombatController = GetComponent<CombatController>();
		MovementController = GetComponent<MovementController>();
	}

    private void FixedUpdate()
    {
		CombatController?.Aim(m_mousePos);
    }

    public override void Damage(float amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0)
			Die();
	}

	public override void Die()
	{
	}
}
