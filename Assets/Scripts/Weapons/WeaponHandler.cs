using UnityEngine;

namespace GardenDefense
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField]
        InputReader _input;
        [SerializeField]
        private Weapon _activeWeapon;
        private void Start()
        {
            Debug.Log($"{_activeWeapon.gameObject.name} is a weapon");
;            _input.Attack += (isAttacking) =>
            {
              if(isAttacking) _activeWeapon.TryShoot();

            };
            _input.Reload += () => _activeWeapon.Reload();
        }
    }
}
