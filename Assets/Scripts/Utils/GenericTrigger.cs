using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnTrigger_Enter;
    private void OnTriggerEnter(Collider other)
    {
        OnTrigger_Enter.Invoke();
    }
}
