using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GenericTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnTrigger_Enter; 
    [SerializeField]
    private UnityEvent OnTrigger_Stay; 
    [SerializeField]
    private UnityEvent OnTrigger_Exit;

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger_Enter.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        OnTrigger_Stay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTrigger_Exit.Invoke();
    }
}
