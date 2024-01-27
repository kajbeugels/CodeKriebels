namespace CodeKriebels.UI.Menu
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;


    public class ControllerIconManager : MonoBehaviour
    {
        [SerializeField, Tooltip("All the controller icons being managed.")]
        private ControllerIcon[] controllerIcons;

        private Dictionary<InputDevice, bool> inputDevices = new Dictionary<InputDevice, bool>();


        private void Awake()
        {
            InputSystem.onDeviceChange += OnDeviceChange;

            for (int i = 0; i < InputSystem.devices.Count; i++)
                OnDeviceChange(InputSystem.devices[i], InputDeviceChange.Added);
        }

        private void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
        {
            if (inputDeviceChange == InputDeviceChange.Added || inputDeviceChange == InputDeviceChange.Reconnected || inputDeviceChange == InputDeviceChange.Enabled)
            {
                if (!inputDevices.ContainsKey(inputDevice))
                    inputDevices.Add(inputDevice, true);

                inputDevices[inputDevice] = true;
            }
            else if (inputDeviceChange == InputDeviceChange.Removed || inputDeviceChange == InputDeviceChange.Disabled)
                inputDevices[inputDevice] = false;

            UpdateControllerIcons();
        }

        private void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        /// <summary>
        /// Updates all the controller icons.
        /// </summary>
        private void UpdateControllerIcons()
        {
            int i = 0;

            // First control the icons.
            foreach (KeyValuePair<InputDevice, bool> inputDevice in inputDevices)
            {
                controllerIcons[i].ToggleControllerIcon(inputDevice.Value);
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