using UnityEngine;

[CreateAssetMenu(fileName = "RoomSettings", menuName = "Rooms/Settings")]
public class RoomTemplateSO : ScriptableObject
{
    public Vector2 roomSpawnLocationOffsets;
    
    public LayerMask roomLayerMask;
    
    public GameObject verticalWall;
    public GameObject verticalDoor;

    public GameObject horizontalWall;
    public GameObject horizontalDoor;
}
