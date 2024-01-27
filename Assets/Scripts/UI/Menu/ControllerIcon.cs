namespace CodeKriebels.UI.Menu
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ControllerIcon : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("The actual icon")]
        private Image controllerImage;

        [SerializeField, Tooltip("The color of the icon when there is no controller.")]
        private Color noControllerColor;

        [SerializeField, Tooltip("The color of the icon when there is a controller.")]
        private Color yesControllerColor;


        /// <summary>
        /// Toggles the controller icon's status.
        /// </summary>
        /// <param name="isActivated">Is the controller icon activated?</param>
        internal void ToggleControllerIcon(bool isActivated)
        {
            controllerImage.color = isActivated ? yesControllerColor : noControllerColor;
        }
    }
}