namespace CodeKriebels.Player
{
    using CodeKriebels.Audio;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.VFX;

    public class PlayerFart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("A reference to the visual effect.")]
        private VisualEffect visualEffect;

        [SerializeField, Tooltip("The interval for farts")]
        private Vector2 fartInterval = new Vector2(0.5f, 1f);


        private void Awake()
        {
            StartCoroutine(DoRandomFart());
        }

        private IEnumerator DoRandomFart()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(fartInterval.x, fartInterval.y));

                visualEffect.Play();
                FartHandler.Instance.PlayFart(FartHandler.FartSize.Small);
            }
        }

        internal void ExecuteFart(FartHandler.FartSize size)
        {
            visualEffect.Play();
            FartHandler.Instance.PlayFart(size);
        }
    }
}