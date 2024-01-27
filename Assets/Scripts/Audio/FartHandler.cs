namespace CodeKriebels.Audio
{
    using NaughtyAttributes;
    using UnityEngine;

    public class FartHandler : MonoBehaviour
    {
        private static FartHandler instance;
        internal static FartHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new FartHandler();

                return instance;
            }
        }

        internal enum FartSize
        {
            Small, Medium, Large
        }

        [Header("References")]
        [SerializeField]
        private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField, Tooltip("All the small farts.")]
        private AudioClip[] smallFarts;

        [SerializeField, Tooltip("All the medium farts.")]
        private AudioClip[] mediumFarts;

        [SerializeField, Tooltip("All the large farts.")]
        private AudioClip[] largeFarts;


#if UNITY_EDITOR
        [Button("Play small fart")]
        private void EDITOR_PlaySmallFart()
        {
            PlayFart(FartSize.Small);
        }

        [Button("Play medium fart")]
        private void EDITOR_PlayMediumFart()
        {
            PlayFart(FartSize.Medium);
        }

        [Button("Play large fart")]
        private void EDITOR_PlayLargeFart()
        {
            PlayFart(FartSize.Large);
        }

#endif

        /// <summary>
        /// Plays a fart through the audio source.
        /// </summary>
        /// <param name="fartSize">The fart size.</param>
        internal void PlayFart(FartSize fartSize)
        {
            switch (fartSize)
            {
                case FartSize.Small:
                    audioSource?.PlayOneShot(smallFarts[Random.Range(0, smallFarts.Length)]);
                    break;
                case FartSize.Medium:
                    audioSource?.PlayOneShot(mediumFarts[Random.Range(0, mediumFarts.Length)]);
                    break;
                case FartSize.Large:
                    audioSource?.PlayOneShot(largeFarts[Random.Range(0, largeFarts.Length)]);
                    break;
            }
        }
    }
}