using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public float maxVelocity = 7.0f;
    public float acceleration = 0.5f;
    public float decceleration = 1.0f;
    public float friction = 1.0f;
    public float rotateSpeed = 10.0f;
    public int maxPlayers = 4;
}
