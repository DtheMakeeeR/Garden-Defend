using TMPro;
using UnityEngine;

namespace GardenDefense
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField]
        InputReader _input;
        [SerializeField]
        private Weapon _activeWeapon;
        [SerializeField]
        private TMP_Text _ammoText;

        private bool _attackPressed;
        private void Start()
        {
            Debug.Log($"{_activeWeapon.gameObject.name} is a weapon");
            ; _input.Attack += (isAttacking) =>
            {
                _attackPressed = isAttacking;

            };
            _input.Reload += () =>
            {
                _activeWeapon.Reload();
            };
            _activeWeapon.Callback += UpdateAmmoText;
            UpdateAmmoText();
        }

        private void UpdateAmmoText()
        {
            _ammoText.text = $"{_activeWeapon.CurrentAmmo} / {_activeWeapon.StoredAmmo}";
        }

        private void Update()
        {
            if( _attackPressed )
            {
                _activeWeapon.TryShoot();
                UpdateAmmoText();
            }
        }
    }
}
