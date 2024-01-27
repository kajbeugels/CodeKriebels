using UnityEngine;
using UnityEngine.Timers;

public class Telephone : MonoBehaviour
{
    [SerializeField]
    private bool isRinging = false;


    [SerializeField]
    private int maxTime;
    [SerializeField]
    private int minTime;

    private void Start()
    {
        
        if (TimerManager.Instance)
        {
            Debug.LogError("No timerManager found");
        }

        TimerManager.Instance.AddTimer(ToggleRinging, Random.Range(minTime, maxTime));
    }

    private void ToggleRinging()
    {
        isRinging = !isRinging;
        TimerManager.Instance.AddTimer(ToggleRinging, Random.Range(minTime, maxTime));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isRinging && other.TryGetComponent(out PlayerMovement player))
        {
        }
    }
}
