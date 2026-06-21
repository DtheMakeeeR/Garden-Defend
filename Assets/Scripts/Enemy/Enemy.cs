using System;
using UnityEngine;

namespace GardenDefense
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        public int Health;
        [SerializeField]
        public int Damage;
        
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
