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
        if (m_hasSpawnedRoom)
            return;

        RoomGenerator.GenerateRoom(m_roomSpawnPos);
        m_hasSpawnedRoom = true;
    }
}
