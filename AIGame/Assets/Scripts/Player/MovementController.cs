using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
	// Player Settings
	public float speed = 0.1f;
	public float acceleration = 5.0f;

	public bool canMove = true;

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
		if (canMove)
			Move();
	}

	private Vector2 m_moveVelocity = Vector2.zero;
	private void Move()
	{
		m_moveVelocity = Vector2.Lerp(m_moveVelocity, m_moveInput * speed, Time.fixedDeltaTime * acceleration);
		m_rigidbody.MovePosition(m_rigidbody.position + m_moveVelocity);
	}

	public void MoveTo(Vector2 position, float time)
    {
		if (isMovingTowards == null)
			isMovingTowards = StartCoroutine(MoveTowards(position, time));
	}

	private Coroutine isMovingTowards = null;
	private IEnumerator MoveTowards(Vector2 position, float time)
    {
		canMove = false;

		float currentTime = 0;
		Vector3 startPos = transform.position;
		Vector3 endPos = new Vector3(position.x, position.y, transform.position.z);

		while (currentTime < time)
		{
			transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(currentTime / time));
			yield return null;
			currentTime += Time.deltaTime;
		}

		transform.position = endPos;
		isMovingTowards = null;

		canMove = true;
	}
}
