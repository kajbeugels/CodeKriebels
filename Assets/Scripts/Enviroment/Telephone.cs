using UnityEngine;
using UnityEngine.Timers;

public class Telephone : MonoBehaviour
{
    [SerializeField]
    private bool isRinging = false;

    #region RingTime
    [SerializeField]
    [Tooltip("How long does the telephone ring!")]
    private float minRingTime;
    [SerializeField]
    private float maxRingTime;
    #endregion
    #region StunTime
    [SerializeField]
    [Tooltip("How long are you stunned!")]
    private float minStunTime; 
    [SerializeField]
    private float maxStunTime;
    #endregion

    private void Start()
    {
        if (!TimerManager.Instance)
        {
            Debug.LogError("No timerManager found");
        }

        TimerManager.Instance.AddTimer(ToggleRinging, Random.Range(minRingTime, maxRingTime));
    }

    private void ToggleRinging()
    {
        isRinging = !isRinging;
        TimerManager.Instance.AddTimer(ToggleRinging, Random.Range(minRingTime, maxRingTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRinging && other.transform.root.TryGetComponent(out PlayerMovement player))
        {
            isRinging = !isRinging;
            float time = Random.Range(minStunTime, maxStunTime);
            //player.DoStun(time);
            TimerManager.Instance.AddTimer(ToggleRinging, time + Random.Range(minRingTime, maxRingTime));
        }
    }
}
