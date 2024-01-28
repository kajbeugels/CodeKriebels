namespace CodeKriebels.Player
{
    using CodeKriebels.Audio;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerFart : MonoBehaviour
    {
        [Header("References")]

        [SerializeField,Tooltip("Reference to the PlayerToSprite component, used to know the looking direction of the player")]
        private PlayerToSprite playerToSprite;

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

        public UnityEvent<FartHandler.FartSize> onEpicFartDooDoo;


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

                onEpicFartDooDoo?.Invoke(FartHandler.FartSize.Small);
            }
        }

        internal void ExecuteFart(FartHandler.FartSize size)
        {
            PlayerToSprite.Direction direction = playerToSprite.GetCurrentDirection();
            switch (direction)
            {
                case PlayerToSprite.Direction.SE:
                    particleSystem.transform.localEulerAngles = new Vector3(0,90,0);
                    break;
                case PlayerToSprite.Direction.NE:
                    particleSystem.transform.localEulerAngles = new Vector3(0, 180, 0);
                    break;
                case PlayerToSprite.Direction.NW:
                    particleSystem.transform.localEulerAngles = new Vector3(0, 270, 0);
                    break;
                case PlayerToSprite.Direction.SW:
                    particleSystem.transform.localEulerAngles = new Vector3(0, 0, 0);
                    break;
            }

            particleSystem.Play();
            FartHandler.Instance.PlayFart(size);
            parent.ExecuteHapticFeedback(hapticLowFrequency, hapticHighFrequency, hapticDuration);
        }
    }
}