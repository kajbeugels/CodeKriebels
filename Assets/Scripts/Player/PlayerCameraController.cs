using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{

    internal Camera camera;
    private Transform pivot;
    private GameManager gameManager;

    public Vector3 DesiredAngles;

    private void Awake ()
    {
        gameManager = FindObjectOfType<GameManager>();
        pivot = transform.GetChild(0);
        camera = GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {
        pivot.rotation = Quaternion.Euler(DesiredAngles);
    }
}
