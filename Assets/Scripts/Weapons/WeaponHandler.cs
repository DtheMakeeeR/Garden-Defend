using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace GardenDefense
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField]
        InputReader _input;
        [SerializeField]
        private Weapon _activeWeapon;
        [SerializeField]
        private Weapon _previousWeapon;
        [SerializeField]
        private List<Weapon> _weapons;
        [SerializeField]
        private TMP_Text _ammoText;

        private bool _attackPressed;
        private bool _reloadPressed;
        private int _weaponIndex;
        private bool CanSwap => !_activeWeapon.IsReloading;
        private void Start()
        {
            ; _input.Attack += (isAttacking) =>
            {
                _attackPressed = isAttacking;
            };
            _input.Reload += isReloading =>
            {
                _reloadPressed = isReloading;
            };
            _input.NextItem += isScrolling =>
            {
                if(isScrolling && CanSwap) IncreaseIndex();
            };
            _input.PreviousItem += isScrolling =>
            {
                if (isScrolling && CanSwap) DecreaseIndex();
            };
            _weaponIndex = 0;
            _activeWeapon = _weapons[_weaponIndex];
            foreach ( Weapon weapon in _weapons )
            {
                weapon.Callback += UpdateAmmoText;
            }
            UpdateAmmoText();
        }

        private void DecreaseIndex()
        {
            _weaponIndex = (_weaponIndex - 1 + _weapons.Count) % _weapons.Count;
            ChangeWeapon();
        }

        private void IncreaseIndex()
        {
            _weaponIndex = (_weaponIndex + 1) % _weapons.Count;
            ChangeWeapon();
        }

        private void ChangeWeapon()
        {
            Debug.Log($"CHANGE WEAPON old active {_activeWeapon.gameObject.name} old previous {_previousWeapon?.gameObject.name ?? "nothing" }");
            _previousWeapon = _activeWeapon;
            _activeWeapon = _weapons[_weaponIndex];
            _previousWeapon.gameObject.SetActive(false);
            _activeWeapon.gameObject.SetActive(true);
            Debug.Log($"CHANGE WEAPON new active {_activeWeapon.gameObject.name} new previous {_previousWeapon.gameObject.name}");
            UpdateAmmoText();
        }

        private void UpdateAmmoText()
        {
            //Debug.Log($"{_activeWeapon.gameObject.name} updating ammo text: {_activeWeapon.CurrentAmmo} / {_activeWeapon.StoredAmmo}");
            _ammoText.text = $"{_activeWeapon.CurrentAmmo} / {_activeWeapon.StoredAmmo}";
        }

        private void Update()
        {
            if( _attackPressed )
            {
                _activeWeapon.TryShoot();
                //UpdateAmmoText();
            }
            if(_reloadPressed)
            {
                Debug.Log("RELOAD REALISED");
                _activeWeapon.Reload();
                _reloadPressed = false;
            }
        }
    }
}
