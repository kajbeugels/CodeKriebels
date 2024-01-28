using CodeKriebels.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSound : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] Clips;
    private float timestamp;

    /// <summary>
    /// Called when something enters a trigger on this object
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        //Check if player hit the trigger, if so, he wins
        if (other.gameObject.GetComponent<Player>() is Player player && Time.time >= timestamp)
        {
            AudioSource.clip = Clips[Random.Range(0, Clips.Length - 1)];
            AudioSource.Play();
            timestamp = Time.time + Random.Range(1.5f,3);
        }
    }
}
