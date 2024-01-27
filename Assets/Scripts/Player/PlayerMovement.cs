using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public enum PlayerMoveState : int
{
    None = 0,
    Bounce = 1,
    Stunned = 2,
}

public class PlayerMovement : MonoBehaviour
{
    internal CharacterController CharacterController { get; private set; }
    private PlayerInput playerInput;
    public Vector3 Offset;
    public Rigidbody Rigidbody;

    private GameManager gameManager;
    private Vector3 inputDirection;
    private Vector3 moveVector;
    private Quaternion playerRotation;
    private Coroutine currentBounceCoroutine;
    private Coroutine currentStunCoroutine;
    public PlayerMoveState moveState;

    private bool CanApplyInput => moveState == PlayerMoveState.None;
    private Vector2 GetMoveInput => playerInput.actions.FindAction("Move").ReadValue<Vector2>();


    private Vector3 latestForward;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        CharacterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        inputDirection = transform.forward;
        playerRotation = transform.rotation;
    }

    private IEnumerator StunCoroutine(float time)
    {
        moveState |= PlayerMoveState.Stunned;

        moveVector *= 0;

        yield return new WaitForSeconds(time);

        moveState &= ~PlayerMoveState.Stunned;
    }

    private IEnumerator BounceCoroutine(Vector3 normal)
    {
        moveState |= PlayerMoveState.Bounce;

        inputDirection = Vector3.Reflect(inputDirection.normalized, normal);
        playerRotation = Quaternion.Euler(0, 0, 0);
        moveVector = inputDirection * moveVector.magnitude;

        transform.rotation = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);

        yield return new WaitForSeconds(gameManager.playerSettings.bounceTime);

        moveState &= ~PlayerMoveState.Bounce;
    }

    public void DoStun(float time)
    {
        if (currentStunCoroutine != null) StopCoroutine(currentStunCoroutine);

        currentStunCoroutine = StartCoroutine(StunCoroutine(time));
    }

    private void DoBounce(Vector3 normal)
    {
        if (currentBounceCoroutine != null) StopCoroutine(currentBounceCoroutine);

        currentBounceCoroutine = StartCoroutine(BounceCoroutine(normal));
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!moveState.HasFlag(PlayerMoveState.Bounce) && hit.normal.y < 0.5f)
        {
            DoBounce(hit.normal);
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = GetMoveInput;
        Vector3 peop = Vector3.zero;

        if (input.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(input.y, -input.x) * Mathf.Rad2Deg;
            Vector3 f = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 forward = Quaternion.Euler(Offset) * f * gameManager.playerSettings.maxVelocity;

            float d = Mathf.Clamp01(Vector3.Dot(Rigidbody.velocity, f.normalized));
            latestForward = forward * (1 - d);


            Rigidbody.MoveRotation(Quaternion.LookRotation(latestForward, Vector3.up));
            peop = latestForward;
            //latestForward = latestForward.normalized * Mathf.Max(latestForward.magnitude, 1);
        }

        Rigidbody.AddForce(peop, ForceMode.VelocityChange);
    }

}