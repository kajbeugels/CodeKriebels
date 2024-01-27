namespace CodeKriebels.Player
{
    using CodeKriebels.Audio;
    using System.Collections;
    using UnityEngine;

    public class PlayerFart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("A reference to the particle system.")]
        private ParticleSystem particleSystem;

        [SerializeField, Tooltip("The interval for farts")]
        private Vector2 fartInterval = new Vector2(0.5f, 1f);

        [Header("Haptic Settings")]
        [SerializeField, Tooltip("The left motor haptic feedback speed.")]
        private float hapticLowFrequency = 0.25f;

        [SerializeField, Tooltip("The right motor haptic feedback speed.")]
        private float hapticHighFrequency = 0.75f;

        [SerializeField, Tooltip("The haptic duration.")]
        private float hapticDuration = 1f;

        internal Player parent;


        private void Awake()
        {
            StartCoroutine(DoRandomFart());
        }

        private IEnumerator DoRandomFart()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(fartInterval.x, fartInterval.y));

                ExecuteFart(FartHandler.FartSize.Small);
            }
        }

        internal void ExecuteFart(FartHandler.FartSize size)
        {
            particleSystem.Play();
            FartHandler.Instance.PlayFart(size);
            parent.ExecuteHapticFeedback(hapticLowFrequency, hapticHighFrequency, hapticDuration);
        }
    }
}