using UnityEngine;

public class Door : MonoBehaviour
{

	private bool m_hasSpawnedRoom = false;
	private Vector2 m_roomSpawnPos;

	public void Initialize(Vector2 position)
	{
		m_roomSpawnPos = position;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!m_hasSpawnedRoom)
		{
			RoomGenerator.GenerateRoom(Vector2Int.RoundToInt(m_roomSpawnPos));
			m_hasSpawnedRoom = true;
		}

		float timeToComplete = 1.0f;
		CameraController.MoveTo(m_roomSpawnPos, timeToComplete);

		if (collision.TryGetComponent(out MovementController movementController))
		{
			Vector2 targetPos = (m_roomSpawnPos - (Vector2)transform.position).normalized * 1.5f;
			movementController.MoveTo((Vector2)transform.position + targetPos, timeToComplete);
		}
	}
}
