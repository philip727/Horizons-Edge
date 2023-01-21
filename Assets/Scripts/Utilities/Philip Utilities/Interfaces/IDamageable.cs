using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    [SerializeField] int MaxHealth { get; }

    [SerializeField] int Health { get; }

    void TakeDamage(int damage);

    void Death();
}
