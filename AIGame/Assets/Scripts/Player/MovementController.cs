using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
	// Player Settings
	public float speed = 0.1f;
	public float acceleration = 5.0f;

	// Component Cache
	private Rigidbody2D m_rigidbody = null;
	
	// Input Cache
	private Vector2 m_moveInput = Vector2.zero;
	
	// Input Fuctions
	public void OnMove(InputAction.CallbackContext context) => m_moveInput = context.ReadValue<Vector2>();
	
	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
	{
		Move();
	}

	private Vector2 m_moveVelocity = Vector2.zero;
	private void Move()
	{
		m_moveVelocity = Vector2.Lerp(m_moveVelocity, m_moveInput * speed, Time.fixedDeltaTime * acceleration);
		m_rigidbody.MovePosition(m_rigidbody.position + m_moveVelocity);
	}
}
