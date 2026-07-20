using UnityEngine;

namespace GardenDefense
{
    public class RailgunWeapon : Weapon
    {
        [SerializeField]
        private float _knockbackForce = 1f;
        [SerializeField]
        private float _accuracySpread = 0f;
        [SerializeField]
        public LayerMask shootableLayers;
        protected override void Shoot()
        {
            Debug.Log($"{gameObject.name} make shot");
            RaycastHit hit;
            Vector3 shootOrigin = Camera.main.transform.position;
            Vector3 shootDirection = Camera.main.transform.forward;

            if (_accuracySpread > 0)
            {
                shootDirection = Quaternion.Euler(
                    Random.Range(-_accuracySpread, _accuracySpread),
                    Random.Range(-_accuracySpread, _accuracySpread),
                    0
                ) * shootDirection;
            }

            Debug.DrawRay(shootOrigin, shootDirection * range, Color.green, 2f);
            if (Physics.Raycast(shootOrigin, shootDirection, out hit, range, shootableLayers))
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
            else Debug.Log($"Hit: nothing");
        }
    }
}
