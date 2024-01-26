using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{


    private Camera camera;
    private Transform pivot;
    private GameManager gameManager;

    private void Awake ()
    {
        gameManager = FindObjectOfType<GameManager>();
        pivot = transform.GetChild(0);
        camera = GetComponentInChildren<Camera>();
    }


}
