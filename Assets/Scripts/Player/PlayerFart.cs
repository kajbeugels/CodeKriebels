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

        [SerializeField, Tooltip("A reference to the particle system.")]
        public ParticleSystem streamParticleSystem;

        [SerializeField, Tooltip("The interval for farts")]
        private Vector2 fartInterval = new Vector2(0.5f, 1f);

        [Header("Haptic Settings")]
        [SerializeField, Tooltip("The left motor haptic feedback speed.")]
        public float hapticLowFrequency = 0.25f;

        [SerializeField, Tooltip("The right motor haptic feedback speed.")]
        public float hapticHighFrequency = 0.75f;

        [SerializeField, Tooltip("The haptic duration.")]
        public float hapticDuration = 1f;

        internal Player parent;

        public UnityEvent<FartHandler.FartSize> onEpicFartDooDoo;


        private void Awake()
        {
            //StartCoroutine(DoRandomFart());
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
            particleSystem.transform.localEulerAngles = GetFartRotation();
            particleSystem.Play();
            FartHandler.Instance.PlayFart(size);
            parent.ExecuteHapticFeedback(hapticLowFrequency, hapticHighFrequency, hapticDuration);
        }

        internal Vector3 GetFartRotation()
        {
            PlayerToSprite.Direction direction = playerToSprite.GetCurrentDirection();
            Vector3 vec = Vector2.zero;

            switch (direction)
            {
                case PlayerToSprite.Direction.SE:
                    vec = new Vector3(0, -232, 0);
                    break;
                case PlayerToSprite.Direction.NE:
                    vec = new Vector3(0, -330, 0);
                    break;
                case PlayerToSprite.Direction.NW:
                    vec = new Vector3(0, -165, 0);
                    break;
                case PlayerToSprite.Direction.SW:
                    vec = new Vector3(0, -60, 0);
                    break;
            }


            return vec;
        }
    }
}