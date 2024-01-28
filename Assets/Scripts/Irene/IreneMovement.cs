using System.Collections;
using UnityEngine;

public class IreneMovement : MonoBehaviour
{
    public float maxVelocity = 1f;
    public Vector2 moveTimes = new Vector2(2, 5f);
    public Transform areaOfInterest;
    public float areaOfInterestRadius = 4f;

    private Rigidbody Rigidbody;
    private Vector3 targetPostion;

    internal Vector3 MoveVector { get; private set; }


    private void GetRandomTargetPosition()
    {
        var offset = Random.insideUnitSphere;
        offset = new Vector3(offset.x, 0f, offset.z) * 5f;

        targetPostion = transform.position + offset;

        var areaOfInterestDistance = Vector3.Distance(transform.position, areaOfInterest.position);

        targetPostion = Vector3.Lerp(targetPostion, areaOfInterest.position, Mathf.Sqrt(areaOfInterestDistance / areaOfInterestRadius));
    }

    private void Awake()
    {
        StartCoroutine(GetRandomOffsetCoroutine());
        Rigidbody = GetComponent<Rigidbody>();
    }

    private IEnumerator GetRandomOffsetCoroutine()
    {
        while (true)
        {
            GetRandomTargetPosition();

            yield return new WaitForSeconds(Random.Range(moveTimes.x, moveTimes.y));
        }
    }

    private void FixedUpdate()
    {
        Vector3 input = targetPostion - transform.position;

        input = Mathf.Clamp01(input.magnitude) * input.normalized;

        MoveVector = input * maxVelocity * (1 - Mathf.Clamp01(Vector3.Dot(Rigidbody.velocity, input.normalized)));

        Rigidbody.AddForce(transform.InverseTransformVector(MoveVector), ForceMode.VelocityChange);

        Debug.DrawLine(transform.position, targetPostion, Color.red, Time.deltaTime);
        Debug.DrawLine(transform.position, transform.position + MoveVector.normalized, Color.blue, Time.deltaTime);
    }
}
