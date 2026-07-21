using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace GardenDefense
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerActions();
    }
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public UnityAction<Vector2> Move = delegate { };
        public UnityAction<bool> Jump = delegate { };
        public UnityAction<bool> Attack = delegate { };
        public UnityAction<bool> Reload = delegate { };
        public UnityAction<bool> Interact = delegate { };
        public UnityAction<bool> Sprint = delegate { };
        public UnityAction<bool> NextItem = delegate { };
        public UnityAction<bool> PreviousItem = delegate { };
        public UnityAction Lock = delegate { };
        public UnityAction<bool> Dodge = delegate { };

        InputSystem_Actions _inputActions;
        public Vector2 Direction => _inputActions.Player.Move.ReadValue<Vector2>();

        public void EnablePlayerActions()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputSystem_Actions();
                _inputActions.Player.SetCallbacks(this);
            }
            _inputActions.Enable();
        }
        public void DisablePlayerActions()
        {
            if (_inputActions != null)
            {
                _inputActions.Disable();
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Attack?.Invoke(true);
                    break;
                default:
                    Attack?.Invoke(false);
                    break;
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Interact?.Invoke(true);
                    break;
                default:
                    Interact?.Invoke(false);
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Jump?.Invoke(true);
                    break;
                default:
                    Jump?.Invoke(false);
                    break;
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Sprint?.Invoke(true);
                    break;
                default:
                    Sprint?.Invoke(false);
                    break;
            }
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Reload?.Invoke(true);
                    break;
                default:
                    Reload?.Invoke(false);
                    break;
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    NextItem?.Invoke(true);
                    break;
                default:
                    NextItem?.Invoke(false);
                    break;
            }
        }

        public void OnPreviousItem(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    PreviousItem?.Invoke(true);
                    break;
                default:
                    PreviousItem?.Invoke(false);
                    break;
            }
        }
    }
}