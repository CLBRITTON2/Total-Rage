using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform thePlayerHead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // LateUpdate is called every frame after all of the other updates have been called
    private void LateUpdate()
    {
        transform.position = thePlayerHead.position;
        transform.rotation = thePlayerHead.rotation;
    }
}
