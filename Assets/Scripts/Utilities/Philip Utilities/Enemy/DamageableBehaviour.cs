using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableBehaviour : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MaxHealth { private set; get; }
    [field: SerializeField] public int Health { private set; get; }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Death();
        }
    }
}
