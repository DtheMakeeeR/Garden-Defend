using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField]
        protected float range;
        [SerializeField]
        protected int damage;
        [SerializeField]
        protected float fireRate;
        [SerializeField]
        protected bool canShoot = true;
        [SerializeField]
        protected float reloadTime;
        [SerializeField]
        protected bool isReloading;
        [SerializeField]
        protected int magSize;
        [SerializeField]
        protected int currentAmmo;
        [SerializeField]
        protected int maxAmmo;
        [SerializeField]
        protected int storedAmmo;


        public IEnumerator<float> _ReloadCoroutine()
        {
            yield return Timing.WaitForSeconds(reloadTime);
            int ammoNeeded = magSize - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, storedAmmo);
            currentAmmo += ammoToReload;
            RemoveAmmo(ammoToReload);

            isReloading = false;
        }

        public IEnumerator<float> _FireRateCoroutine()
        {
            canShoot = false;
            yield return Timing.WaitForSeconds(fireRate);
            canShoot = true;
        }

        public abstract void Shoot();
        public void Reload()
        {
            if(currentAmmo < magSize && storedAmmo > 0 && !isReloading)
            {
                isReloading = true;
                Timing.RunCoroutine(_ReloadCoroutine().CancelWith(gameObject));
            }
        }
        public void AddAmmo(int amount)
        {
            storedAmmo = Mathf.Min(storedAmmo + amount, maxAmmo);
        }

        public void RemoveAmmo(int amount)
        {
            storedAmmo -= amount;
        }

        public void TryShoot()
        {
            if(currentAmmo > 0 && !isReloading && canShoot)
            {
                Timing.RunCoroutine(_FireRateCoroutine().CancelWith(gameObject));
                Shoot();
                currentAmmo--;
            }
            else if (currentAmmo <= 0 && storedAmmo > 0 && !isReloading)
            {
                Reload();
            }
        }
    }
}
