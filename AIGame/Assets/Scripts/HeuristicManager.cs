using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
    private RoomGenerator rg;
    [SerializeField] private int startingDifficulty = 10;
    [SerializeField] private int difficultyIncrement = 5;
    private int currDifficulty;

    // MAIN BIG BOY
    public void assignDifficulty()
    {

    }


    private void increaseDifficulty()
    {
        print("New Difficulty: " + currDifficulty);
        currDifficulty += difficultyIncrement;
    }

    void Start()
    {
        RoomGenerator.onRoomGenerated.AddListener(increaseDifficulty);
    }

}
