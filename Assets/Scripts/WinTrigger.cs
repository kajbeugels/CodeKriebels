using CodeKriebels.Player;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    /// <summary>
    /// Called when something enters a trigger on this object
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        //Check if player hit the trigger, if so, he wins
        if (other.GetComponent<Player>() is Player player)
        {
            GameManager.Instance.CameraSetings.ToggleCameraSettings(true);
            GameManager.Instance.EndGameplay(player);
        }
    }
}
