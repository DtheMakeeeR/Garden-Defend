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

        [Header("Ammo")]
        [SerializeField]
        protected int magSize;
        [SerializeField]
        protected int currentAmmo;
        [SerializeField]
        protected int maxAmmo;
        [SerializeField]
        protected int storedAmmo;
        public int CurrentAmmo

        {
            get => currentAmmo;
            set
            {
                currentAmmo = Mathf.Clamp(value, 0, magSize);
                Callback?.Invoke();
            }
        }
        public int StoredAmmo
        {
            get => storedAmmo;
            set
            {
                storedAmmo = Mathf.Clamp(value, 0, maxAmmo);
                Callback?.Invoke();
            }
        }

        public delegate void AmmoCallback();

        public event AmmoCallback Callback;
        private void Awake()
        {
            Debug.Log($"*** {currentAmmo}/{storedAmmo} fields");
        }
        public IEnumerator<float> _ReloadCoroutine()
        {
            yield return Timing.WaitForSeconds(reloadTime);
            int ammoNeeded = magSize - CurrentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, StoredAmmo);
            CurrentAmmo += ammoToReload;
            StoredAmmo -= ammoToReload;

            isReloading = false;
            Debug.Log($"{gameObject.name} is reloaded. Ammo in mag: {CurrentAmmo}");
        }

        public IEnumerator<float> _FireRateCoroutine()
        {
            canShoot = false;
            yield return Timing.WaitForSeconds(fireRate);
            canShoot = true;
        }

        protected abstract void Shoot();
        public void Reload()
        {
            if(CurrentAmmo < magSize && StoredAmmo > 0 && !isReloading)
            {
                isReloading = true;
                Debug.Log($"{gameObject.name} is reloading. Ammo in mag: {CurrentAmmo}");
                Timing.RunCoroutine(_ReloadCoroutine().CancelWith(gameObject));
            }
        }

        public void TryShoot()
        {
            if(CurrentAmmo > 0 && !isReloading && canShoot)
            {
                Timing.RunCoroutine(_FireRateCoroutine().CancelWith(gameObject));
                Shoot();
                CurrentAmmo--;
            }
            else if (CurrentAmmo <= 0 && StoredAmmo > 0 && !isReloading)
            {
                Reload();
            }
        }
    }
}
