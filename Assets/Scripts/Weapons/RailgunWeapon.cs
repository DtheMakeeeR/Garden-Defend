using UnityEngine;

namespace GardenDefense
{
    public class RailgunWeapon : Weapon
    {
        [SerializeField]
        private float _knockbackForce = 1f;
        [SerializeField]
        public LayerMask shootableLayers;
        protected override void Shoot()
        {
            Debug.Log($"{gameObject.name} make shot");
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range, shootableLayers))
            {
                Debug.Log($"Hit: {hit.collider.name}");
                var health = hit.collider.GetComponent<Entity>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                var rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(-hit.normal * _knockbackForce); 
                }
            }
        }
    }
}
