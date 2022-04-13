using UnityEngine;
using System.Collections;

public class Enemy : EntityBase
{
    private Rigidbody2D rb2d;
    private IEnumerator attackCoroutine;
    public GameObject projectile;

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

    public void GenerateEnemy(EnemySO stats, int difficulty)
    {
        int newHealth = Random.Range(1, difficulty);

        maxHealth = newHealth;
        currentHealth = maxHealth;

        maxSpeed = stats.maxSpeed;
        maxAccel = maxSpeed * 2;
        targetSpeed = maxSpeed * 2;
        timeToTarget = 0.1f;
        slowRadius = 2;
        targetRadiusBuffer = 0.2f;
        rotationSpeed = 1;

        weapon = stats.weapon;
    }

    /* // Seek
        steeringParams result = new steeringParams();
        result.linear = targetPos - (Vector2)transform.position;
        result.linear.Normalize();
        result.linear *= maxAccel;

        result.angular = 0;

        return result;
        */

    /*
    steeringParams GetWander()
    {
        steeringParams result = new steeringParams();
        //result.linear = maxSpeed * gameObject;
        result.angular = Random.Range(-1f, 1f) - Random.Range(-1f, 1f);
    }
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

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            //weapon.Attack(1.0f);
            GameObject newProj = Instantiate(projectile, transform.position + transform.up, transform.rotation);

            Vector2 direction = target.position - transform.position;
            direction.Normalize();

            newProj.GetComponent<Rigidbody2D>().AddForce(direction * 5, ForceMode2D.Impulse);

            print("attacked");
            print(newProj);
        }
    }

    /*
    private void Attack()
    {
        weapon.Attack(1.0f);
    }
    */
    #endregion

    public override void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) { Die(); }
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

        weapon = new Bow();

        attackCoroutine = Attack();
        StartCoroutine(attackCoroutine);
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

        Vector3 diff = targetPos - (Vector2)transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

    }

    public void LateUpdate()
    {
        // Update Variables
        targetPos = target.position;
        //rotation = newSteer.angular;
    }
    #endregion

}
