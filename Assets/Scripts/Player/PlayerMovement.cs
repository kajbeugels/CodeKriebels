using CodeKriebels.Player;
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
    private PlayerFart playerFarts;
    private PlayerInput playerInput;
    public Vector3 Offset;
    public Rigidbody Rigidbody;
    public float BounceForce;
    [SerializeField]
    private AnimationCurve fartChargeCurve;
    [SerializeField]
    private AnimationCurve afterFartStunCurve;
    public float FartChargeScalar;

    private GameManager gameManager;
    private Coroutine currentBounceCoroutine;
    private Coroutine currentStunCoroutine;
    public PlayerMoveState moveState;

    private Vector2 GetMoveInput => playerInput.actions.FindAction("Move").ReadValue<Vector2>();


    private Vector3 latestMovementDirection;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
    }
    
    private IEnumerator StunCoroutine(float time)
    {
        moveState |= PlayerMoveState.Stunned;

        yield return new WaitForSeconds(time);

        moveState &= ~PlayerMoveState.Stunned;
    }

    private IEnumerator BounceCoroutine(Vector3 normal)
    {
        moveState = PlayerMoveState.Bounce;

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.AddForce(normal * Mathf.Min(BounceForce, 2.5f), ForceMode.Impulse);

        playerFarts?.ExecuteFart(CodeKriebels.Audio.FartHandler.FartSize.Large);

        yield return new WaitForSeconds(gameManager.playerSettings.bounceTime);

        moveState = PlayerMoveState.None;
    }

    private IEnumerator FartChargeCoroutine()
    {
        bool inputPressed = playerInput.actions.FindAction("FartCharge").IsPressed();
        float chargeTime = 0f;

        moveState = PlayerMoveState.Stunned;

        while (inputPressed)
        {
            chargeTime += Time.deltaTime;
            inputPressed = playerInput.actions.FindAction("FartCharge").IsPressed();
            yield return null;
        }

        //Get total charge force from curve sampled by time
        float force = fartChargeCurve.Evaluate(chargeTime) * FartChargeScalar;
        Rigidbody.AddForce(force * latestMovementDirection.normalized, ForceMode.Impulse);
        playerFarts.ExecuteFart(CodeKriebels.Audio.FartHandler.FartSize.Large);

        yield return new WaitForSeconds(afterFartStunCurve.Evaluate(chargeTime));
        moveState = PlayerMoveState.None;
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

    void OnCollisionEnter(Collision coll)
    {
        if (moveState == PlayerMoveState.Bounce)
            return;

        Vector3 n = coll.contacts[0].normal;
        if (!moveState.HasFlag(PlayerMoveState.Bounce) && n.y < 0.5f)
        {
            DoBounce(n * coll.impulse.magnitude);
        }
    }

    private void FixedUpdate()
    {
        if (moveState != PlayerMoveState.None)
            return;

        Vector2 playerInput = GetMoveInput;
        Vector3 inputVector = Vector3.zero;

        if (playerInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(playerInput.y, -playerInput.x) * Mathf.Rad2Deg;
            Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 adjustedForward = Quaternion.Euler(Offset) * forward * gameManager.playerSettings.maxVelocity;

            latestMovementDirection = adjustedForward * (1 - Mathf.Clamp01(Vector3.Dot(Rigidbody.velocity, forward.normalized)));
            Rigidbody.MoveRotation(Quaternion.LookRotation(latestMovementDirection, Vector3.up));

            inputVector = latestMovementDirection;
        }

        Rigidbody.AddForce(inputVector, ForceMode.VelocityChange);
    }

}