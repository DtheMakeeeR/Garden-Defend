using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace GardenDefense
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Basic Settings")]
        [SerializeField]
        protected float range;
        [SerializeField]
        protected int damage;
        [SerializeField]
        protected float fireRate;
        [SerializeField]
        protected float reloadTime;
        //[SerializeField]
        [Header("Flags")]
        [SerializeField]
        protected bool canShoot = true;
        public bool IsReloading { get; private set; }

        [Header("Ammo")]
        [SerializeField]
        protected int magSize;
        [SerializeField]
        protected int currentAmmo;
        [SerializeField]
        protected int maxAmmo;
        [SerializeField]
        protected int storedAmmo;

        [Header("Sounds")]
        [SerializeField]
        protected AudioSource _audioSource;
        [SerializeField]
        protected AudioClip _shotSound;
        [SerializeField]
        protected float _minPitch;
        [SerializeField]
        protected float _maxPitch;
        [SerializeField]
        protected AudioClip _reloadSound;
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

            IsReloading = false;
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
            if(CurrentAmmo < magSize && StoredAmmo > 0 && !IsReloading)
            {
                PlayReloadSound();
                IsReloading = true;
                Debug.Log($"{gameObject.name} is reloading. Ammo in mag: {CurrentAmmo}");
                Timing.RunCoroutine(_ReloadCoroutine().CancelWith(gameObject));
            }
        }

        public void TryShoot()
        {
            if(CurrentAmmo > 0 && !IsReloading && canShoot)
            {
                Timing.RunCoroutine(_FireRateCoroutine().CancelWith(gameObject));
                PlayShotSound();
                Shoot();
                CurrentAmmo--;
            }
            else if (CurrentAmmo <= 0 && StoredAmmo > 0 && !IsReloading)
            {
                Reload();
            }
        }

        private void PlayReloadSound()
        {
            _audioSource.pitch = 1f;
            _audioSource.PlayOneShot(_reloadSound);
        }

        private void PlayShotSound()
        {
            _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
            _audioSource.PlayOneShot(_shotSound);
        }
    }
}
