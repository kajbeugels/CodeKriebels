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
    private CharacterController characterController;
    private PlayerInput playerInput;
    public Vector3 Offset;

    private GameManager gameManager;
    private Vector3 inputDirection;
    private Vector3 moveVector;
    private Quaternion playerRotation;
    private Coroutine currentBounceCoroutine;
    private Coroutine currentStunCoroutine;
    public PlayerMoveState moveState;

    private bool CanApplyInput => moveState == PlayerMoveState.None;
    private Vector2 GetMoveInput => playerInput.actions.FindAction("Move").ReadValue<Vector2>();

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        characterController = GetComponent<CharacterController>();
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

    private void Update()
    {
        if (CanApplyInput)
        {
            var moveAction = GetMoveInput;

            if (moveAction.magnitude > 0.05f && CanApplyInput)
            {
                inputDirection = new Vector3(moveAction.x * 0.77f, 0, moveAction.y).normalized;
                inputDirection = Quaternion.Euler(Offset) * inputDirection;
                    
                Debug.DrawLine(transform.position, transform.position + inputDirection, Color.red);

                var angleBetween = Vector3.SignedAngle(inputDirection, transform.forward, Vector3.up);

                var rotationInput = -angleBetween / 180f;

                if (float.IsNaN(rotationInput)) rotationInput = 0;

                playerRotation *= Quaternion.Euler(0, rotationInput * gameManager.playerSettings.rotateSpeed * Time.deltaTime, 0);
            }

            var maxVelocity = gameManager.playerSettings.maxVelocity;
            var acceleration = gameManager.playerSettings.acceleration;
            var decceleration = gameManager.playerSettings.decceleration;
            var friction = gameManager.playerSettings.friction;
            var rotateSpeed = gameManager.playerSettings.rotateSpeed;

            var moveDirection = playerRotation * inputDirection;

            moveDirection = moveDirection.normalized;

            var desiredSpeed = moveDirection.magnitude * maxVelocity;
            var dot = Vector3.Dot(moveDirection, moveVector);

            var speed = (float)Mathf.Sqrt(moveVector.x * moveVector.x + moveVector.z * moveVector.z);
            var speedMul = speed - (speed < decceleration ? decceleration : speed) * friction * Time.deltaTime;

            if (speedMul < 0) speedMul = 0;
            if (speed > 0) speedMul /= speed;

            if (CanApplyInput) moveVector *= speedMul;

            var velAdd = desiredSpeed - dot;
            var velMul = acceleration * Time.deltaTime * desiredSpeed;

            if (velMul > velAdd) velMul = velAdd;

            if (CanApplyInput) moveVector += moveDirection * velMul;
        }

        print(CanApplyInput);

        characterController.Move(transform.InverseTransformVector(moveVector) * Time.deltaTime);
        transform.rotation = playerRotation;
    }

}