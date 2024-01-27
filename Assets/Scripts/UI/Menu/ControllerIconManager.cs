namespace CodeKriebels.UI.Menu
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;


    public class ControllerIconManager : MonoBehaviour
    {
        [SerializeField, Tooltip("All the controller icons being managed.")]
        private ControllerIcon[] controllerIcons;


        private IEnumerator Start()
        {
            while (PlayerInputManager.instance == null)
                yield return null;

            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;

            UpdateControllerIcons();
        }

        private void OnPlayerLeft(PlayerInput obj)
        {
            UpdateControllerIcons();
        }

        private void OnPlayerJoined(PlayerInput obj)
        {
            UpdateControllerIcons();
        }

        /// <summary>
        /// Updates all the controller icons.
        /// </summary>
        private void UpdateControllerIcons()
        {
            int i = 0;

            // First control the icons.
            while (i < PlayerInputManager.instance.playerCount)
            {
                controllerIcons[i].ToggleControllerIcon(true);
                i++;
            }

            // Then disable the rest.
            while (i < controllerIcons.Length)
            {
                controllerIcons[i].ToggleControllerIcon(false);
                i++;
            }
        }
    }
}