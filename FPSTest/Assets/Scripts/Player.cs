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
    public Transform firePosition;

    public GameObject muzzleFlash, bulletHole;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerFirstPersonView();
        FireWeapon();
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

        // Disables look inversion. If you move your mouse up you will look up
        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);

        // Apply local rotation to only rotate the camera
        // Quaternion is used for rotation
        // Euler returns vector 3 converted into rotations
        mainCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
    private void FireWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Raycast is determining what the bullet just hit, the origin and direction
            // are based of where the player is looking
            RaycastHit hit;

            if(Physics.Raycast(mainCameraHead.position, mainCameraHead.forward, out hit, 100f))
            {
                // Managing bullet accuracy based off distance and point of aim
                float distance = Vector3.Distance(mainCameraHead.position, hit.point);

                if (distance > 2f)
                {
                    firePosition.LookAt(hit.point);
                    Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            else
            {
                // If the bullet hits nothing it still has a direction to fire
                firePosition.LookAt(mainCameraHead.position + (mainCameraHead.forward * 50f));
            }

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);

            GameObject bullet = ObjectPoolManager.instance.GetPooledObject("Bullet");
            if(bullet != null)
            {
                bullet.transform.position = firePosition.position;
                bullet.transform.rotation = firePosition.rotation;
                bullet.SetActive(true);
            }
            return;
        }
    }
}
