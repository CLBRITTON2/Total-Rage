using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform ThePlayerHead;
    public float FieldOfViewZoomSpeed = 13f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // LateUpdate is called every frame after all of the other updates have been called
    private void LateUpdate()
    {
        transform.position = ThePlayerHead.position;
        transform.rotation = ThePlayerHead.rotation;
    }
}
