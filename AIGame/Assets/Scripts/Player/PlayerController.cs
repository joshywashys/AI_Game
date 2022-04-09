public class PlayerController : EntityBase
{
    public CombatController CombatController { get; private set; } = null;
    public MovementController MovementController { get; private set; } = null;

    private void Awake()
    {
        CombatController = GetComponent<CombatController>();
        MovementController = GetComponent<MovementController>();
    }

    public override void Damage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    public override void Die()
    {
    }
}
