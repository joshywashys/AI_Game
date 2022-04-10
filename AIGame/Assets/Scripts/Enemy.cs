using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    private Rigidbody2D rb2d;

    #region Steering
    [Header("Heuristic Stats")]
    public float maxSpeed = 15;
    public float maxAccel = 30;
    public float targetSpeed = 15;
    public float timeToTarget = 0.1f;
    public float slowRadius = 2;
    public float targetRadius = 5;
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
    
    /* // Seek
        steeringParams result = new steeringParams();
        result.linear = targetPos - (Vector2)transform.position;
        result.linear.Normalize();
        result.linear *= maxAccel;

        result.angular = 0;

        return result;
        */

    steeringParams GetFlee()
    {
        steeringParams result = new steeringParams();
        result.linear = (Vector2)transform.position - targetPos;
        result.linear.Normalize();
        result.linear *= maxAccel;

        result.angular = 0;

        return result;
    }

    steeringParams GetArrive()
    {
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
        slowRadius = targetRadius + 2;
    }

    public void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        
        steeringParams newSteer = GetArrive();

        // Steer
        if (newSteer.linear != new Vector2(0, 0)) //if arrived at location
        {
            velocity += newSteer.linear * Time.fixedDeltaTime;
            rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);
        }
        else if (distance < targetRadius - targetRadiusBuffer)
        {
            newSteer = GetFlee();
            velocity += newSteer.linear * Time.fixedDeltaTime;
            rb2d.MovePosition(rb2d.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            velocity = Vector2.zero;
        }
    }

    public void LateUpdate()
    {
        // Update Variables
        targetPos = target.position;
        //rotation = newSteer.angular;
    }
    #endregion

}
