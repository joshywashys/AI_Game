using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EntityBase>() != null)
        {
            EntityBase entity = collision.gameObject.GetComponent<EntityBase>();
            entity?.Damage(damage);
        }
    }
}
