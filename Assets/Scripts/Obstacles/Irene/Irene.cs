namespace CodeKriebels.Obstacles
{
    using System.Collections;
    using UnityEngine;

    public class Irene : MonoBehaviour
    {
        [SerializeField, Tooltip("How long should the player be stunned when touching her?")]
        private float playerStunTime = 1;

        [SerializeField, Tooltip("The time the speech bubble is active.")]
        private float speechBubbleDuration = 1;
        private Coroutine speechBubbleCoroutine;

        [SerializeField]
        private IreneVisuals visuals;


        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.GetComponentInChildren<PlayerMovement>() is PlayerMovement playerMovement)
            {
                playerMovement.DoStun(playerStunTime);

                if (speechBubbleCoroutine != null)
                    StopCoroutine(speechBubbleCoroutine);

                speechBubbleCoroutine = StartCoroutine(DoActivateSpeechBubble());
            }
        }

        private IEnumerator DoActivateSpeechBubble()
        {
            visuals.ToggleSpeechBubble(true);
            yield return new WaitForSeconds(speechBubbleDuration);
            visuals.ToggleSpeechBubble(false);
        }
    }
}