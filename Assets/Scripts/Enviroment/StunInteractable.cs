using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timers;

public class StunInteractable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent startStunning;
    [SerializeField]
    private UnityEvent endStunning;
    
    [SerializeField]
    private float stunTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.DoStun(stunTime);
        }
    }
}
