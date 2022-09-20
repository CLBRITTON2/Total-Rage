using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = FindObjectOfType<CameraMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + _mainCamera.forward);
    }
}
