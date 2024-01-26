using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private InputActions inputActions;

    private GameManager gameManager;
    public Vector3 moveVector;

    private Vector2 GetMoveInput => playerInput.actions.FindAction("Move").ReadValue<Vector2>();

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        var moveAction = GetMoveInput;

        var maxVelocity = gameManager.playerSettings.maxVelocity;
        var acceleration = gameManager.playerSettings.acceleration;
        var decceleration = gameManager.playerSettings.decceleration;
        var friction = gameManager.playerSettings.friction;
        var rotateSpeed = gameManager.playerSettings.rotateSpeed;

        var rotationInput = moveAction.x;

        var playerRotation = Quaternion.Euler(0, rotationInput, 0);
        var moveDirection = playerRotation * transform.forward;

        moveDirection = moveDirection.normalized;

        var moveSpeed = moveDirection.magnitude * maxVelocity;
        var dot = Vector3.Dot(moveDirection, moveVector);

        var speed = (float)Mathf.Sqrt(moveVector.x * moveVector.x + moveVector.z * moveVector.z);
        var speedMul = speed - (speed < decceleration ? decceleration : speed) * friction * Time.deltaTime;

        if (speedMul < 0) speedMul = 0;
        if (speed > 0) speedMul /= speed;

        moveVector *= speedMul;

        var velAdd = moveSpeed - dot;
        var velMul = acceleration * Time.deltaTime * moveSpeed;

        if (velMul > velAdd) velMul = velAdd;

        moveVector += moveDirection * velMul;

        if (moveVector.y > -0.5f) moveVector.y = -0.5f; // Make sure there is always a little gravity to keep the character on the ground.    }
    
        characterController.Move(moveVector * Time.deltaTime);

        transform.Rotate(new Vector3(0, rotationInput * rotateSpeed * Time.deltaTime,0), Space.Self);


        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red, Time.deltaTime);
    }

}