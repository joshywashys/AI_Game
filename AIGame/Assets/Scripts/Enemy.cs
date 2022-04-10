using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    float maxSpeed;
    float maxAccel;

    // Tracking player
    public Transform target;


    // Attacking player
    public WeaponBase weapon;
    //public float attackRange; //(get from weapons)
    
    

    public override void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) { Die();  }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    private void Attack()
    {
        //uses weapon
    }
}
