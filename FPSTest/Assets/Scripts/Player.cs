using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController playerController;
    public Transform mainCameraHead;
    private float speed = 18.0f;
    public float mouseSensitivity = 700f;
    private float cameraVerticalRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerFirstPersonView();
    }
    private void PlayerMovement()
    {
        // get the horizontal and vertical axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Combine the x and z axis, transform the Player object (inside the unity inspector "Transform")
        Vector3 move = x * transform.right + z * transform.forward;

        // Multiply the transforming x and y axis by the speed to control player movement speed
        // Detach the player velocity from the frame rate by multiplying move by Time.deltaTime
        move = move * speed * Time.deltaTime;

        playerController.Move(move);
    }
    private void PlayerFirstPersonView()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        cameraVerticalRotation -= mouseY;
    }
}
