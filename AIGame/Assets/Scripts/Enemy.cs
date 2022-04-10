using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    Rigidbody2D rb2d;

    #region Steering
    public float maxSpeed = 8;
    public float maxAccel = 1;
    public float slowRadius = 1;
    public float targetRadius = 1;
    public float rotationSpeed = 1;
    private Vector2 velocity;

    struct steeringParams
    {
        public Vector2 linear;
        public float angular;
    }
    steeringParams steer;

    steeringParams GetSteering()
    {
        steeringParams result = new steeringParams();
        result.linear = targetPos - (Vector2)transform.position;
        result.linear.Normalize();
        result.linear *= maxAccel;

        result.angular = 0;

        return result;
    }


    private Vector2 targetPos;
    public Transform target;
    public void GetPlayer()
    {
        Transform player = FindObjectOfType<PlayerController>().transform;
        if (player != null)
        {
            target = player;
        }
        else
        {
            target = transform;
        }
    }


    #endregion

    #region Attacking
    // Attacking player
    public WeaponBase weapon;
    //public float attackRange; //(get from weapons)


    private void Attack()
    {
        //weapon.Attack();
    }
    #endregion

    public override void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) { Die();  }
    }

    public override void Die()
    {
        Destroy(gameObject);
    }

    #region Monobehaviour Functions

    public void Start()
    {
        // Initialize Variables
        if (target == null) { GetPlayer(); } //ignore player if target set in editor
        velocity = Vector2.zero;
        rb2d = GetComponent<Rigidbody2D>();
        targetRadius = weapon.;
        slowRadius = maxSpeed * 2 + targetRadius;
    }

    public void FixedUpdate()
    {
        // Update Variables
        targetPos = target.position;
        steeringParams newSteer = GetSteering();

        velocity += newSteer.linear * Time.fixedDeltaTime;
        //rotation = newSteer.angular;

        // Steer
        //transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position, Vector3.up), Time.deltaTime * rotationSpeed);  //rotation * Time.deltaTime;
    }
    #endregion

}
