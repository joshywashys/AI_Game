using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	[SerializeField] private int startingDifficulty = 10;
	[SerializeField] private int difficultyIncrement = 5;
	public static int currDifficulty;

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
		currDifficulty = startingDifficulty;
		RoomGenerator.onRoomGenerated.AddListener(increaseDifficulty);
	}

}
