using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : EntitySO
{
    public float maxSpeed;
    public WeaponBase weapon;

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
    }
    
}
