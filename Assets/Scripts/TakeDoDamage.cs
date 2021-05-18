using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TakeDoDamage : MonoBehaviour, IDamageable<int>
{
    [Header("Health and Damage")]
    public int currentHealthEnemy;
    [SerializeField] protected int damage;
    //private bool EnemyDead;
    
    //public virtual void TakeDamage(int value)
    //{
    //    currentHealthEnemy -= value;

    //    ExecuteHitAnim();

    //    if (currentHealthEnemy <= 0)
    //    {
    //        ExecuteDeathAnim();
    //        Invoke(nameof(DestroyEnemy), 12f);
    //    }
    //}

    protected virtual void ExecuteDeathAnim()
    {
        
    }

    protected virtual void ExecuteHitAnim()
    {
        
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int value)
    {
        currentHealthEnemy -= value;

        ExecuteHitAnim();

        if (currentHealthEnemy <= 0)
        {
            ExecuteDeathAnim();
            Invoke(nameof(DestroyEnemy), 12f);
        }
    }
}

public interface IDamageable<T>
{
    void TakeDamage(T value);
}
