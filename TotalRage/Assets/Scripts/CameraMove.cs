using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform ThePlayerHead;
    private float _startFieldOfView, _targetFieldOfView;
    public float FieldOfViewZoomSpeed = 13f;
    private Camera _theCamera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _theCamera = GetComponent<Camera>();

        _startFieldOfView = _theCamera.fieldOfView;
        _targetFieldOfView = _startFieldOfView;
    }

    // LateUpdate is called every frame after all of the other updates have been called
    private void LateUpdate()
    {
        transform.position = ThePlayerHead.position;
        transform.rotation = ThePlayerHead.rotation;

        _theCamera.fieldOfView = Mathf.Lerp(_theCamera.fieldOfView, _targetFieldOfView, FieldOfViewZoomSpeed * Time.deltaTime);
    }
    public void ZoomIn(float targetZoom)
    {
        _targetFieldOfView = targetZoom;
    }
    public void ZoomOut()
    {
        _targetFieldOfView = _startFieldOfView;
    }
}
