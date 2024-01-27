using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private InputActions inputActions;

    private GameManager gameManager;
    public Vector3 inputDirection;
    public Vector3 moveVector;
    public Quaternion playerRotation;

    private Vector2 GetMoveInput => playerInput.actions.FindAction("Move").ReadValue<Vector2>();

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        inputDirection = transform.forward;
        playerRotation = transform.rotation;
    }

    private void Update()
    {
        var moveAction = GetMoveInput;

        if (moveAction.magnitude > 0.05f)
        {
            inputDirection = new Vector3(moveAction.x, 0, moveAction.y);
        }

        var maxVelocity = gameManager.playerSettings.maxVelocity;
        var acceleration = gameManager.playerSettings.acceleration;
        var decceleration = gameManager.playerSettings.decceleration;
        var friction = gameManager.playerSettings.friction;
        var rotateSpeed = gameManager.playerSettings.rotateSpeed;

        var angleBetween = Vector3.SignedAngle(inputDirection, transform.forward, Vector3.up);

        var rotationInput = -angleBetween / 180f;// Mathf.Sqrt(angleBetween / 180f) * angleSign;

        if (float.IsNaN(rotationInput)) rotationInput = 0;

        playerRotation *= Quaternion.Euler(0, rotationInput, 0);
        var moveDirection = playerRotation * inputDirection;

        moveDirection = moveDirection.normalized;

        var desiredSpeed = moveDirection.magnitude * maxVelocity;
        var dot = Vector3.Dot(moveDirection, moveVector);

        var speed = (float)Mathf.Sqrt(moveVector.x * moveVector.x + moveVector.z * moveVector.z);
        var speedMul = speed - (speed < decceleration ? decceleration : speed) * friction * Time.deltaTime;

        if (speedMul < 0) speedMul = 0;
        if (speed > 0) speedMul /= speed;

        moveVector *= speedMul;

        var velAdd = desiredSpeed - dot;
        var velMul = acceleration * Time.deltaTime * desiredSpeed;

        if (velMul > velAdd) velMul = velAdd;

        moveVector += moveDirection * velMul;

        characterController.Move(transform.InverseTransformVector(moveVector) * Time.deltaTime);
        transform.rotation = playerRotation;
  
        Debug.DrawLine(transform.position, transform.position + inputDirection, Color.yellow, Time.deltaTime);
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red, Time.deltaTime);
        Debug.DrawLine(transform.position, transform.position + transform.InverseTransformVector(moveVector), Color.green, Time.deltaTime);
    }

}