using CodeKriebels.Audio;
using CodeKriebels.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public ParticleSystem StunParticles;

    [SerializeField, Tooltip("The audio source belonging to the stun sound effect.")]
    private AudioSource stunAudioSource;

    public Transform FartIndicatorPivot;
    public MeshRenderer FartIndicator;
    public MeshRenderer FartIndicatorBase;

    public Transform PoopEmoji;
    public Transform start, end;

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
        FartIndicatorBase.material.SetColor("_PlayerColor", PlayerManager.Instance.PlayerColors[PlayerManager.Instance.GetPlayerIndex(Player)]);
    }

    private IEnumerator StunCoroutine(float time)
    {
        StunParticles.Play();
        stunAudioSource.Play();

        moveState = PlayerMoveState.Stunned;
        Rigidbody.velocity = Vector3.zero;

        yield return new WaitForSeconds(time);

        moveState = PlayerMoveState.None;
        StunParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        stunAudioSource.Stop();
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

        PoopEmoji.gameObject.SetActive(true);
        PoopEmoji.transform.position = start.position;
        PoopEmoji.transform.localScale = start.localScale;

        while (inputPressed)
        {
            chargeTime += Time.deltaTime;
            inputPressed = playerInput.actions.FindAction("ChargeFart").IsPressed();
            FartIndicator.material.SetFloat("_Percentage", chargeTime);


            Vector3 position = Vector3.Lerp(start.position, end.position, chargeTime + (Time.deltaTime * 4));
            Vector3 scale = Vector3.Lerp(start.localScale, end.localScale, chargeTime + (Time.deltaTime * 4));

            PoopEmoji.transform.position = position;
            PoopEmoji.transform.localScale = scale;

            yield return null;
        }

        playerFarts.onEpicFartDooDoo.Invoke(FartHandler.FartSize.Large);
        playerFarts.streamParticleSystem.transform.localEulerAngles = playerFarts.GetFartRotation();
        playerFarts.streamParticleSystem.Play();

        //Get total charge force from curve sampled by time
        float force = fartChargeCurve.Evaluate(chargeTime) * FartChargeScalar;

        Rigidbody.AddForce(force * FartIndicatorPivot.transform.forward.normalized, ForceMode.Impulse);

        FartIndicator.material.SetFloat("_Percentage", 0);
        PoopEmoji.gameObject.SetActive(false);




        FartHandler.Instance.PlayFart(FartHandler.FartSize.Small);
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