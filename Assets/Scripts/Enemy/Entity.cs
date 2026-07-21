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
                Debug.Log($"{gameObject.name} has died.");
                if (OnDeath != null) Debug.Log($"{gameObject.name} has OnDeath event.");
                else Debug.Log($"{gameObject.name} hasnt OnDeath event.");
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
