using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	[SerializeField] private int startingDifficulty = 10;
	[SerializeField] private int difficultyIncrement = 5;
	public static int currDifficulty;

	// MAIN BIG BOY, makes setup for next room
	public static List<EnemySO> GetEnemies(int difficulty)
	{
        int numEnemies = Random.Range(1, difficulty);
        int enemyDiff = difficulty / ((numEnemies > 0) ? numEnemies : 1);

        EnemySO newEnemySO = EnemySO.GenerateEnemySO(enemyDiff);
		
        List<EnemySO> enemies = new List<EnemySO>();
        for (int i = 0; i < numEnemies; i++)
            enemies.Add(newEnemySO);
        
		print(enemies.Count);

        return enemies;
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
