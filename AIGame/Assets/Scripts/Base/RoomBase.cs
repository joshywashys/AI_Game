using System.Collections.Generic;
using UnityEngine;

public class RoomBase : MonoBehaviour
{
	public GameObject[] enemyPrefab;
	
	public RoomTemplateSO roomTemplate;
	public bool roomCleared = true;

	public List<Enemy> enemies = new List<Enemy>();
	private Dictionary<Vector2Int, bool> doors = new Dictionary<Vector2Int, bool>
	{
		{ new Vector2Int( 0,  1), false },
		{ new Vector2Int( 1,  0), false },
		{ new Vector2Int( 0, -1), false },
		{ new Vector2Int(-1,  0), false }
	};

	public bool RoomHasDoor(Vector2 current)
	{
		Vector2Int direction = Vector2Int.RoundToInt(current.normalized);
		return doors[direction];
	}

	public void ShutDoors() => ControlDoors(true);
	public void OpenDoors() => ControlDoors(false);
	
	private void ControlDoors(bool closed)
	{
		Door[] doorComponents = GetComponentsInChildren<Door>();
		foreach (Door door in doorComponents)
		{
			door.trigger.enabled = !closed;
			door.visual.SetActive(closed);
		}
	}

	private void Start()
	{
		CheckValidRoomLocations();
		RoomGenerator.AddRoom(Vector2Int.RoundToInt(transform.position), this);

		int enemiesToSpawn = Mathf.FloorToInt(HeuristicManager.currDifficulty / 5.0f);
		for (int i = 0; i < enemiesToSpawn; i++)
		{
			int enemyDifficulty = HeuristicManager.currDifficulty;

			List<EnemySO> enemiesList = HeuristicManager.GetEnemies(enemyDifficulty);
			int enemyIndex = Random.Range(0, enemiesList.Count);
			EnemySO enemySO = enemiesList[enemyIndex];

			Enemy newEnemy = Instantiate(enemyPrefab[0], transform.position, Quaternion.identity).GetComponent<Enemy>();
			newEnemy.GenerateEnemy(enemySO, enemyDifficulty);
			enemies.Add(newEnemy);
		}
	}

    private void Update()
    {
        if (enemies.Count <= 0 && !roomCleared)
        {
			OpenDoors();
			roomCleared = true;
			CameraController.onTransitionComplete.RemoveListener(ShutDoors);
        }
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
			float sign = (int)Mathf.Sign(i - 2);
			bool even = i % 2 == 0;

			Vector2 checkPosition = (even ? horizontalWallOffset : verticalWallOffset) * sign;
			RoomBase adjacentRoom = RoomGenerator.GetRoom(Vector2Int.RoundToInt((Vector2)transform.position + checkPosition));
			bool doorAvailable = false;
			if (adjacentRoom)
			{
				doorAvailable = adjacentRoom.RoomHasDoor(-checkPosition);
			}
			else
			{
				availableRoomIndices.Add(i);

				if (Random.Range(0, 2) == 0)
				{
					doorAvailable = true;
					hasNewDoorSpawned = true;

					Vector2Int direction = Vector2Int.RoundToInt(checkPosition.normalized);
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
