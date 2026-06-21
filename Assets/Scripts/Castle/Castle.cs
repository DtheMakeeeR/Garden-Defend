using System;
using UnityEngine;

namespace GardenDefense
{
    public class Castle : MonoBehaviour
    {
        [SerializeField]
        public int Health;

        private void OnCollisionEnter(Collision collision)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.Damage);
                enemy.TakeDamage(enemy.Health); 
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
