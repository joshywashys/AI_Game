using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    Rigidbody2D rb2d;

    #region Steering
    public float maxSpeed = 15;
    public float maxAccel = 10;
    public float targetSpeed = 15;
    public float timeToTarget = 0.1f;
    public float slowRadius = 3;
    public float targetRadius = 2;
    public float targetRadiusBuffer = 0.2f;
    public float rotationSpeed = 1;
    private Vector2 velocity;
    private Vector2 targetVelocity;

    struct steeringParams
    {
        public Vector2 linear;
        public float angular;
    }
    steeringParams steer;

    steeringParams GetSteering()
    {
        /* // Seek
        steeringParams result = new steeringParams();
        result.linear = targetPos - (Vector2)transform.position;
        result.linear.Normalize();
        result.linear *= maxAccel;

        result.angular = 0;

        return result;
        */

        // Arrive
        steeringParams result = new steeringParams();
        result.linear = new Vector2(0,0);
        result.angular = 0;

        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        if (distance < targetRadius) { return result; }
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * ((distance - targetRadius) / slowRadius);
        }

        targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        result.linear = targetVelocity - velocity;
        result.linear /= timeToTarget;

        if (result.linear.magnitude > maxAccel)
        {
            result.linear.Normalize();
            result.linear *= maxAccel;
        }
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
        if (weapon != null) { targetRadius = weapon.desiredRange - targetRadiusBuffer; }
        slowRadius = maxSpeed + targetRadius;
    }

    public void FixedUpdate()
    {
        // Steer
        //transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        steeringParams newSteer = GetSteering();
        velocity += newSteer.linear * Time.fixedDeltaTime;

        if (newSteer.linear != new Vector2(0, 0))
        {
            rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            velocity = new Vector2(0, 0);
        }
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position, Vector3.up), Time.deltaTime * rotationSpeed);  //rotation * Time.deltaTime;
    }

    public void LateUpdate()
    {
        // Update Variables
        targetPos = target.position;
        //rotation = newSteer.angular;
    }
    #endregion

}
