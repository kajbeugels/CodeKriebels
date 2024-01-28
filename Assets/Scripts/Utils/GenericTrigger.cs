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
        if (other.gameObject.name.Contains("Player"))
        {
            OnTrigger_Enter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            OnTrigger_Stay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            OnTrigger_Exit.Invoke();
        }
    }
}
