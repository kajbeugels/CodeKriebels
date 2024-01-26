using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public float maxVelocity = 1.0f;
    public float acceleration = 1.0f;
    public float friction = 1.0f;
    public int maxPlayers = 4;
}
