using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : EntitySO
{
    public float maxSpeed;
    public Bow weapon;

    [Header("Complex Traits")]
    public bool floats; //avoids obstacles, cant get AvoidWalls

    public enum BehaviourTypes
    {
        Wander,
        Arrive
    }

    public enum BehavioursAdditional
    {
        Flee, //impossible if
        AvoidWalls, //avoid walls
        BounceOffWalls //bounce off walls
    }
    
    public static EnemySO GenerateEnemySO(int difficulty)
    {
        EnemySO newEnemySO = CreateInstance<EnemySO>();

        int newHealth = Random.Range(1, difficulty);
        
        newEnemySO.maxHealth = newHealth;
        newEnemySO.maxSpeed = difficulty / newHealth;

        //newEnemySO.weapon = new Bow();

        return newEnemySO;
    }

}
