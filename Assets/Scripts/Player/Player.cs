using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Color color;
    public PlayerInput Input;
    public PlayerMovement PlayerMovement;


    private PlayerCameraController playerCameraController;
    private PlayerController playerController;

    public PlayerMovement Movement => PlayerMovement;
    public PlayerCameraController CameraController => playerCameraController;
    public PlayerController Controller => playerController;


    private void Awake()
    {
        playerCameraController = GetComponentInChildren<PlayerCameraController>();
        playerController = GetComponentInChildren<PlayerController>();
    }
}
