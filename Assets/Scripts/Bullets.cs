using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;

    public void OnTriggerEnter(Collider collision)
    {
        //TakeDoDamage enemy = collision.GetComponent<TakeDoDamage>();
        //if (enemy != null)
        //{
        //    enemy.TakeDamage(damage);
        //    Destroy(gameObject);
        //}

        DamageReceiver receiver = collision.GetComponent<DamageReceiver>();
        if (receiver != null)
        {
            receiver.ApplyDamage(damage);
            Destroy(gameObject);
        }

        if (collision.tag == ("Ground"))
        {
            Destroy(gameObject);
        }

        //EnemyBehaviour enemy = collision.GetComponent<EnemyBehaviour>();
        //if (collision.tag == ("Enemy"))
        //{
        //    enemy.TakeDamage((int)damage);
        //    Destroy(gameObject);
        //}

        //WomanZombie enemyWoman = collision.GetComponent<WomanZombie>();
        //if (collision.tag == ("Enemy2"))
        //{
        //    enemyWoman.TakeDamage((int)damage);
        //    Destroy(gameObject);
        //}
    }
}
