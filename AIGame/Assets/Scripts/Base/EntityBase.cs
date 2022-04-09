using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    public float maxHealth, currentHealth;

    public virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public abstract void Damage(float amount);
    public abstract void Die();
}
