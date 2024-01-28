namespace CodeKriebels.Obstacles
{
    using UnityEngine;

    public class Irene : MonoBehaviour
    {
        [SerializeField, Tooltip("How long should the player be stunned when touching her?")]
        private float playerStunTime = 1;


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponentInChildren<PlayerMovement>() is PlayerMovement playerMovement)
                playerMovement.DoStun(playerStunTime);
        }
    }
}