using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public float maxPitchAdd = 1f;
    public float maxPitchOsc = 0.25f;
    public float maxPitchVib = 50f;
    public float minDistanceForEffect = 20f;

    public Transform targetTransform; 


    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (PlayerManager.Instance.players.Count == 0)
        {
            audioSource.pitch = 1f;

            return;
        }

        float averageDistanceToToilet = 0f;

        foreach (var player in PlayerManager.Instance.players)
        {
            averageDistanceToToilet += Vector3.Distance(player.transform.position, targetTransform.position) / PlayerManager.Instance.players.Count;
        }

        var i = 1f - Mathf.Clamp01(averageDistanceToToilet / minDistanceForEffect);

        audioSource.pitch = 1f + i * maxPitchAdd + Mathf.Sin(Time.timeSinceLevelLoad * maxPitchVib * i) * maxPitchOsc * i;
    }
}
