using CodeKriebels.Audio;
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

    public Player Player;
    public PlayerFart playerFarts;
    private PlayerInput playerInput;
    public Vector3 Offset;
    public Rigidbody Rigidbody;
    public float BounceForce;
    [SerializeField]
    private AnimationCurve fartChargeCurve;
    [SerializeField]
    private AnimationCurve afterFartStunCurve;
    public float FartChargeScalar;

    public Transform FartIndicatorPivot;
    public MeshRenderer FartIndicator;

    private GameManager gameManager;
    private Coroutine currentBounceCoroutine;
    private Coroutine currentStunCoroutine;
    private Coroutine currentFartCoroutine;
    public PlayerMoveState moveState;

    private Vector2 GetMoveInput => playerInput.actions.FindAction("Move").ReadValue<Vector2>();

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        FartIndicator.material.SetFloat("_Percentage", 0);
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
        bool inputPressed = playerInput.actions.FindAction("ChargeFart").IsPressed();
        float chargeTime = 0f;

        moveState = PlayerMoveState.Stunned;

        Vector2 input = GetMoveInput;
        float angle = Mathf.Atan2(input.y, -input.x) * Mathf.Rad2Deg;
        Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        Vector3 adjustedForward = Quaternion.Euler(Offset) * forward;

        while (inputPressed)
        {
            chargeTime += Time.deltaTime;
            inputPressed = playerInput.actions.FindAction("ChargeFart").IsPressed();
            FartIndicator.material.SetFloat("_Percentage", chargeTime);
            yield return null;
        }

        playerFarts.onEpicFartDooDoo.Invoke(FartHandler.FartSize.Large);
        playerFarts.streamParticleSystem.transform.localEulerAngles = playerFarts.GetFartRotation();
        playerFarts.streamParticleSystem.Play();

        //Get total charge force from curve sampled by time
        float force = fartChargeCurve.Evaluate(chargeTime) * FartChargeScalar;

        Rigidbody.AddForce(force * FartIndicatorPivot.transform.forward.normalized, ForceMode.Impulse);

        FartIndicator.material.SetFloat("_Percentage", 0);

        FartHandler.Instance.PlayFart(FartHandler.FartSize.Large);
        Player.ExecuteHapticFeedback(playerFarts.hapticLowFrequency, playerFarts.hapticHighFrequency, playerFarts.hapticDuration);

        yield return new WaitForSeconds(afterFartStunCurve.Evaluate(chargeTime));
        playerFarts.streamParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        moveState = PlayerMoveState.None;
        currentFartCoroutine = null;
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

        if (coll.gameObject.CompareTag("Obstacle"))
        {
            playerFarts.ExecuteFart(FartHandler.FartSize.Medium);
            //DoBounce(n * coll.impulse.magnitude);
        }
    }

    private void Update()
    {
        Vector2 input = GetMoveInput;
        float angle = Mathf.Atan2(input.y, -input.x) * Mathf.Rad2Deg;
        Vector3 forward = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        Vector3 adjustedForward = Quaternion.Euler(Offset) * forward;

        FartIndicatorPivot.transform.forward = adjustedForward;

        if (moveState != PlayerMoveState.None)
            return;

        bool inputPressed = playerInput.actions.FindAction("ChargeFart").IsPressed();

        if (inputPressed)
        {
            if (currentFartCoroutine != null)
                return;

            currentFartCoroutine = StartCoroutine(FartChargeCoroutine());
        }
    }
}