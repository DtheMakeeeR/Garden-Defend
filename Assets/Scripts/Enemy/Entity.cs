using System;
using UnityEngine;
using UnityEngine.Events;

namespace GardenDefense
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        public int Health;

        public UnityAction OnDeath;

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                OnDeath?.Invoke();
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
