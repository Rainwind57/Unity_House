using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using Cinemachine; // We need this for the cameras

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        public bool analogMovement;

        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        // --- Camera Switcher Fields ---
        [Header("Camera Objects")]
        public CinemachineVirtualCamera thirdPersonCamera;
        public CinemachineVirtualCamera firstPersonCamera;
        public Camera mainCamera; // The one with the CinemachineBrain

        [Header("Camera Priorities")]
        [Tooltip("Priority when camera is active")]
        public int highPriority = 20;
        [Tooltip("Priority when camera is inactive")]
        public int lowPriority = 10;

        [Header("Culling Masks")]
        [Tooltip("Layers to render in 3rd person (should include 'Player')")]
        public LayerMask thirdPersonMask;
        [Tooltip("Layers to render in 1st person (should EXCLUDE 'Player')")]
        public LayerMask firstPersonMask;

        private bool isFirstPerson = false;
        // --- End Camera Switcher Fields ---

        // --- Added Start Method ---
        private void Start()
        {
            // Start in third-person view
            SetThirdPersonView();
        }
        // --- End Start Method ---


#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		// --- Added Camera Switch Method ---
		public void OnSwitchCamera(InputValue value)
		{
			SwitchCameraInput(value.isPressed);
		}
		// --- End Camera Switch Method ---
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        // --- Added Camera Switch Logic ---
        public void SwitchCameraInput(bool newSwitchState)
        {
            // Only trigger on the button press (not release)
            if (newSwitchState)
            {
                isFirstPerson = !isFirstPerson;

                if (isFirstPerson)
                {
                    SetFirstPersonView();
                }
                else
                {
                    SetThirdPersonView();
                }
            }
        }

        private void SetFirstPersonView()
        {
            firstPersonCamera.Priority = highPriority;
            thirdPersonCamera.Priority = lowPriority;
            mainCamera.cullingMask = firstPersonMask;
        }

        private void SetThirdPersonView()
        {
            firstPersonCamera.Priority = lowPriority;
            thirdPersonCamera.Priority = highPriority;
            mainCamera.cullingMask = thirdPersonMask;
        }
        // --- End Camera Switch Logic ---


        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}