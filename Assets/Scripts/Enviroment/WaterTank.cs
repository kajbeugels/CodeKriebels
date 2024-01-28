using UnityEngine;
using UnityEngine.Events;

public class WaterTank : MonoBehaviour
{
    [SerializeField]
    private float upFactor;
    [SerializeField]
    private GameObject Water;
    [SerializeField]
    private Transform spawnPosition;

    public AudioSource Audio;

    private bool isSpawned = false;
    // Update is called once per frame
    void Update()
    {
        if(transform.up.y < upFactor && !isSpawned)
        {
            Water.SetActive(true);
            Water.transform.position = new Vector3(spawnPosition.position.x,0,spawnPosition.position.z);
            Audio.Play();
            isSpawned = true;
        }
    }
}
