using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	[SerializeField] private int startingDifficulty = 10;
	[SerializeField] private int difficultyIncrement = 5;
	public static int currDifficulty;

	// MAIN BIG BOY, makes setup for next room
	public static List<Enemy> GetEnemies(int difficulty)
	{
        int numEnemies = difficulty / Random.Range(1, difficulty);
        int enemyDiff = difficulty / numEnemies;

        EnemySO newEnemySO = EnemySO.GenerateEnemySO(enemyDiff);
        Enemy newEnemy = Enemy.GenerateEnemy(newEnemySO);

        List<Enemy> enemies = new List<Enemy>();
        for (int i = 0; i < numEnemies; i++)
        {
            enemies.Add(newEnemy);
        }

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
