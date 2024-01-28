namespace CodeKriebels.Obstacles
{
    using UnityEngine;

    public class RatOfDeath : MonoBehaviour
    {
        [SerializeField, Tooltip("How long should the player be stunned when touching her?")]
        private float playerStunTime = 1;


        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.GetComponentInChildren<PlayerMovement>() is PlayerMovement playerMovement)
                playerMovement.DoStun(playerStunTime);
        }
    }
}
