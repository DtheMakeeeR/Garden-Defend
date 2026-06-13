using Unity.VisualScripting;
using UnityEngine;

namespace GardenDefense
{
    public class RotateWithCamera : MonoBehaviour
    {
        [Header("References")]
        public Transform CameraTransform;

        private void Awake()
        {
            if (CameraTransform == null)
            {
                // Пытаемся найти камеру через CinemachineBrain (обычно она главная)
                Camera mainCam = Camera.main;
                if (mainCam != null)
                    CameraTransform = mainCam.transform;
                else
                    Debug.LogError("PlayerRotationFollowCamera: Не найдена камера!");
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (CameraTransform == null) return;

            Vector3 cameraForward = CameraTransform.forward;

            cameraForward.y = 0;

            // 2. Нормализуем направление (чтобы не было искажений)
            cameraForward.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = targetRotation;
        }
    }
}
