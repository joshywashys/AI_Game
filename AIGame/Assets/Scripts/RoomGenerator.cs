using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomGenerator : MonoBehaviour
{
	public static RoomGenerator Instance;
    public static UnityEvent onRoomGenerated;

    public GameObject roomPrefab;

    public int roomCount = 0;

	private static Dictionary<Vector2Int, RoomBase> rooms = new Dictionary<Vector2Int, RoomBase>();

    private void Awake()
    {
        Instance = this;
    }

    public static void AddRoom(Vector2Int position, RoomBase room)
    {
        rooms.Add(position, room);
        Instance.roomCount = rooms.Count;
    }

    public static RoomBase GetRoom(Vector2Int roomPosition)
    {
		if (rooms.TryGetValue(roomPosition, out RoomBase room))
			return room;
		return null;
    }

	public static void GenerateRoom(Vector2Int position)
    {
        if (rooms.ContainsKey(position))
            return;

        Instantiate(Instance.roomPrefab, ((Vector3Int)position), Quaternion.identity);

        onRoomGenerated?.Invoke();
    }
}
