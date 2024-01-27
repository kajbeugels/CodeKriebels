using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReserviour : MonoBehaviour
{
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
