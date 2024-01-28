using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private Camera camera
    {
        get
        {
            return Camera.main;
        }
    }
    private float nearClipPlane;
    private CameraClearFlags clearFlags;
    private Color backgroundColor;

    public void Start()
    {
        CacheCameraSettings();
    }

    public void CacheCameraSettings()
    {
        nearClipPlane = camera.nearClipPlane;
        clearFlags = camera.clearFlags;
        backgroundColor = camera.backgroundColor;
    }

    public void ToggleCameraSettings(bool show)
    {
        if (!show)
        {
            camera.nearClipPlane = 100;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
        }
        else
        {
            camera.nearClipPlane = nearClipPlane;
            camera.clearFlags = clearFlags;
            camera.backgroundColor = backgroundColor;
        }
    }
}
