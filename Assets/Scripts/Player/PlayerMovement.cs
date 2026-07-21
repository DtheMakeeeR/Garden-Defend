using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GardenDefense { 
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Gravity Settings")]
        public float GravityStrength;
        public float GroundedGravityStrength;
        public float JumpStrength;

        [Header("Movement Settings")]
        public float MoveSmoothTime;
        public float WalkSpeed;
        public float RunSpeed;

        private CharacterController _controller;
        [SerializeField]        
        InputReader _input;
        private Vector3 _currentMoveVelocity;
        private Vector3 _MoveDampVelocity;
        private Vector3 _playerInput;
        private Vector3 _moveDirection;

        private Vector3 _currentForceVelocity;

        private bool _isSprinting = false;
        private bool _isJumping = false;

        // Start is called before the first frame update
        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }
        private void Start()
        {
            _input.Move += direction =>
            {
                _playerInput.x = direction.x;
                _playerInput.y = 0f;
                _playerInput.z = direction.y;
            };
            _input.Sprint += isSprinting => _isSprinting = isSprinting;
            _input.Jump += isJumping =>
            {
                _isJumping = isJumping;
            };

            _input.Interact += isInteracting =>
            {
                if (!isInteracting) return;
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            };
            _input.EnablePlayerActions();




        }

        // Update is called once per frame
        void Update()
        {
            float currentSpeed = _isSprinting ? RunSpeed : WalkSpeed;
            _moveDirection = transform.TransformDirection(_playerInput.normalized);
            _currentMoveVelocity = Vector3.SmoothDamp(
                _currentMoveVelocity,
                _moveDirection * currentSpeed,
                ref _MoveDampVelocity,
                MoveSmoothTime
            );


            if (_controller.isGrounded)
            {
                _currentForceVelocity.y -= GroundedGravityStrength;
                if(_isJumping)
                {
                    _currentForceVelocity.y = JumpStrength;
                }
            }
            else
            {
                _currentForceVelocity.y -= GravityStrength * Time.deltaTime;
            }
            _controller.Move(_currentMoveVelocity * Time.deltaTime);
            _controller.Move(_currentForceVelocity * Time.deltaTime);
        }
        private void OnDestroy()
        {
            _input.DisablePlayerActions();
        }
    }
}