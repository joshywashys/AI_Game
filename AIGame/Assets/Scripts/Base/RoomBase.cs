using System.Collections.Generic;
using UnityEngine;

public class RoomBase : MonoBehaviour
{
	public RoomTemplateSO roomTemplate;
	
	private Dictionary<Vector2Int, bool> doors = new Dictionary<Vector2Int, bool>
	{
		{ new Vector2Int( 0,  1), false },
		{ new Vector2Int( 1,  0), false },
		{ new Vector2Int( 0, -1), false },
		{ new Vector2Int(-1,  0), false }
	};

	public bool RoomHasDoor(Vector2 current)
    {
		Vector2Int direction = Vector2Int.RoundToInt(((Vector2)transform.position - current));
		print(((Vector2)transform.position - current).normalized);
		return doors[direction];
    }

    private void Start()
    {
		CheckValidRoomLocations();
		RoomGenerator.AddRoom(transform.position, this);
    }

    public void CheckValidRoomLocations()
	{
		bool hasNewDoorSpawned = false;
		List<int> availableRoomIndices = new List<int>();
		GameObject[] walls = new GameObject[4];

		Vector2 verticalWallOffset = Vector2.right * roomTemplate.roomSpawnLocationOffsets.x;
		Vector2 horizontalWallOffset = Vector2.up * roomTemplate.roomSpawnLocationOffsets.y;

		for (int i = 0; i < 4; i++)
		{
			float sign = Mathf.Sign(i - 2);
			bool even = i % 2 == 0;

			Vector2 checkPosition = (even ? horizontalWallOffset : verticalWallOffset) * sign;
			RoomBase adjacentRoom = RoomGenerator.GetRoom(checkPosition);
			bool doorAvailable = false;
			if (adjacentRoom)
			{
				doorAvailable = adjacentRoom.RoomHasDoor(transform.position);
			}
			else
            {
				availableRoomIndices.Add(i);

				if (Random.Range(0, 2) == 0)
                {
					doorAvailable = true;
					hasNewDoorSpawned = true;

					Vector2Int direction = Vector2Int.RoundToInt(((Vector2)transform.position - checkPosition).normalized);
					doors[direction] = true;
				}
			}

			GameObject wallPrefab;
			if (doorAvailable)
				wallPrefab = even ? roomTemplate.horizontalDoor : roomTemplate.verticalDoor;
			else
				wallPrefab = even ? roomTemplate.horizontalWall : roomTemplate.verticalWall;
		
			Quaternion roomRotation = (sign > 0) ? Quaternion.identity : Quaternion.Euler(Vector3.forward * 180.0f);
			walls[i] = Instantiate(wallPrefab, (Vector2)transform.position + checkPosition / 2.0f, roomRotation, transform);
			if (doorAvailable)
				walls[i].GetComponentInChildren<Door>().Initialize((Vector2)transform.position + checkPosition);
		}

		if (!hasNewDoorSpawned && availableRoomIndices.Count > 0)
        {
			int index = Random.Range(0, availableRoomIndices.Count);
			ForceDoorPlacement(walls[index], index % 2 == 0);
        }
	}

	public void ForceDoorPlacement(GameObject wall, bool even)
    {
		GameObject doorPrefab = even ? roomTemplate.horizontalDoor : roomTemplate.verticalDoor;
		Vector2Int direction = Vector2Int.RoundToInt((wall.transform.position - transform.position).normalized);
		Vector2 roomSpawnLocation = (Vector2)transform.position + direction * roomTemplate.roomSpawnLocationOffsets;
		
		Instantiate(doorPrefab, wall.transform.position, wall.transform.rotation, transform).GetComponentInChildren<Door>().Initialize(roomSpawnLocation);
		doors[direction] = true;
		Destroy(wall);
    }
}
