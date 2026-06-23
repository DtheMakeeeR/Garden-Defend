using System;
using UnityEngine;

namespace GardenDefense
{
    public class Castle : Entity
    {

        private void OnCollisionEnter(Collision collision)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.Damage);
                enemy.TakeDamage(enemy.Health); 
            }
        }
    }
}
