using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchSettings : MonoBehaviour
{
    public Camera Camera;
    private float nearClipPlane;
    private CameraClearFlags clearFlags;
    private Color backgroundColor;
    private int originalMask;

    public void Start()
    {

        CacheCameraSettings();
    }

    public void CacheCameraSettings()
    {
        originalMask = Camera.cullingMask;
        nearClipPlane = Camera.nearClipPlane;
        clearFlags = Camera.clearFlags;
        backgroundColor = Camera.backgroundColor;
    }

    public void ToggleCameraSettings(bool show)
    {
        if (!show) 
        {
            Camera.cullingMask = 0;
            Camera.clearFlags = CameraClearFlags.SolidColor;
        }
        else
        {
            Camera.clearFlags = CameraClearFlags.Skybox;
            Camera.cullingMask = originalMask;
        }
    }
}
