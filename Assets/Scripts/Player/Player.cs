using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;

    private PlayerMovement playerMovement;
    private PlayerCameraController playerCameraController;
    private PlayerController playerController;

    public PlayerMovement Movement => playerMovement;
    public PlayerCameraController CameraController => playerCameraController;
    public PlayerController Controller => playerController;


    private void Awake()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerCameraController = GetComponentInChildren<PlayerCameraController>();
        playerController = GetComponentInChildren<PlayerController>();
    }
}
