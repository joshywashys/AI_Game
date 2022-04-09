using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
	public static RoomGenerator Instance;

    public GameObject roomPrefab;

	private static Dictionary<Vector2, RoomBase> rooms = new Dictionary<Vector2, RoomBase>();

    private void Awake()
    {
        Instance = this;
    }

    public static void AddRoom(Vector2 position, RoomBase room)
    {
        rooms.Add(position, room);
    }

    public static RoomBase GetRoom(Vector2 roomPosition)
    {
		if (rooms.TryGetValue(roomPosition, out RoomBase room))
			return room;
		return null;
    }

	public static void GenerateRoom(Vector2 position)
    {
        if (rooms.ContainsKey(position))
            return;

        Instantiate(Instance.roomPrefab, position, Quaternion.identity);
    }
}
