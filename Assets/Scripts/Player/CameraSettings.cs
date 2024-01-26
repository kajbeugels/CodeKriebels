using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings", order = 1)]
public class CameraSettings : ScriptableObject
{
    public float cameraDistance = 10;
    public float cameraSize = 5;
}
