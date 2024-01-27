using UnityEngine;
using UnityEngine.Events;

public class GenericCollider : MonoBehaviour
{
    [SerializeField]
    public UnityEvent EnterCollision;
    [SerializeField]
    public UnityEvent ExecuteCollision;
    [SerializeField]
    public UnityEvent ExitCollision;
}
