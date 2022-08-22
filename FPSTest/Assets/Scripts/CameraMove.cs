using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform thePlayerHead;
    private float startFieldOfView, targetFieldOfView;
    public float fieldOfViewZoomSpeed = 13f;
    private Camera theCamera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        theCamera = GetComponent<Camera>();

        startFieldOfView = theCamera.fieldOfView;
        targetFieldOfView = startFieldOfView;
    }

    // LateUpdate is called every frame after all of the other updates have been called
    private void LateUpdate()
    {
        transform.position = thePlayerHead.position;
        transform.rotation = thePlayerHead.rotation;

        theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, targetFieldOfView, fieldOfViewZoomSpeed * Time.deltaTime);
    }
    public void ZoomIn(float targetZoom)
    {
        targetFieldOfView = targetZoom;
    }
    public void ZoomOut()
    {
        targetFieldOfView = startFieldOfView;
    }
}
