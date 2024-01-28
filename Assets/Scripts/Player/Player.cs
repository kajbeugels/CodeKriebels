namespace CodeKriebels.Player
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class Player : MonoBehaviour
    {
        public PlayerInput Input;
        public PlayerMovement PlayerMovement;
        internal PlayerFart Fart;
        internal PlayerToSprite PlayerToSprite;

        private Gamepad currentVibratingGamepad;
        private PlayerCameraController playerCameraController;

        public PlayerMovement Movement => PlayerMovement;
        public PlayerCameraController CameraController => playerCameraController;


        private void Awake()
        {
            playerCameraController = GetComponentInChildren<PlayerCameraController>();
        }

        private void OnDestroy()
        {
            if (currentVibratingGamepad != null)
            {
                currentVibratingGamepad.ResetHaptics();
                currentVibratingGamepad.PauseHaptics();
            }
        }

        internal void ExecuteHapticFeedback(float hapticLowFrequency, float hapticHighFrequency, float hapticDuration)
        {
            for (int i = 0; i < Input.devices.Count; i++)
            {
                if (Input.devices[i] is Gamepad gamepad)
                {
                    gamepad.SetMotorSpeeds(hapticLowFrequency, hapticHighFrequency);
                    StartCoroutine(DoExecuteHapticFeedback(gamepad, hapticDuration));
                }
            }

            IEnumerator DoExecuteHapticFeedback(Gamepad gamepad, float hapticDuration)
            {
                currentVibratingGamepad = gamepad;

                gamepad.ResumeHaptics();
                yield return new WaitForSeconds(hapticDuration);
                gamepad.PauseHaptics();

                currentVibratingGamepad = null;
            }
        }
    }
}