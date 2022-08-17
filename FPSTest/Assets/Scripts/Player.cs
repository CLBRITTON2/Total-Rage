using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    ObjectPoolManager objectPooler;
    public CharacterController playerController;
    public Transform mainCameraHead;
    public Transform firePosition;
    public GameObject muzzleFlash;
    public Transform ground;
    public LayerMask groundLayer;

    private float playerMovementSpeed = 15.0f;
    public float mouseSensitivity = 700f;
    private float cameraVerticalRotation;

    public Vector3 velocity;
    public float gravityModifier;

    public float jumpHeight = 10f;
    private bool playerCanJump;
    private bool initializeJump;
    public float groundDistance = 0.5f;

    private Vector3 playerCrouchingScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 playerBodyScale;
    public Transform playerBody;
    private float initialControllerHeight;
    private bool playerIsCrouching;
    private float playerCrouchMovementSpeed = 8f;


    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPoolManager.instance;
        playerBodyScale = playerBody.localScale;
        initialControllerHeight = playerController.height;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerFirstPersonView();
        InitializeJumpCheck();
        FireWeapon();
        PlayerCrouching();
    }
    private void FixedUpdate()
    {
        AddVelocityToPlayer();

        if (initializeJump)
        {
            Jump();
        }
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
        if (playerIsCrouching)
        {
            move = move * playerCrouchMovementSpeed * Time.deltaTime;
        }
        else if (!playerIsCrouching)
        {
            move = move * playerMovementSpeed * Time.deltaTime;
        }

        playerController.Move(move);
    }
    private void PlayerCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCrouching();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            StopCrouching();
        }
    }
    private void StartCrouching()
    {
        playerBody.localScale = playerCrouchingScale;
        mainCameraHead.position -= new Vector3(0, 1f, 0);
        playerController.height /= 2;
        playerIsCrouching = true;
    }
    private void StopCrouching()
    {
        playerBody.localScale = playerBodyScale;
        mainCameraHead.position += new Vector3(0, 1f, 0);
        playerController.height = initialControllerHeight;
        playerIsCrouching = false;
    }
    private void AddVelocityToPlayer()
    {
        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;
        if (playerController.isGrounded)
        {
            velocity.y = Physics.gravity.y * Time.deltaTime;
        }
        playerController.Move(velocity);
    }
    private void InitializeJumpCheck()
    {
        playerCanJump = Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;

        if (Input.GetButtonDown("Jump") && playerCanJump)
        {
            initializeJump = true;
        }
    }
    private void Jump()
    {
        // Height affected by gravity formula
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        playerController.Move(velocity);
        initializeJump = false;
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

            if (Physics.Raycast(mainCameraHead.position, mainCameraHead.forward, out hit, 100f))
            {
                // Managing bullet accuracy based off distance and point of aim
                float distance = Vector3.Distance(mainCameraHead.position, hit.point);
                if (distance > 2f)
                {
                    firePosition.LookAt(hit.point);

                    if (hit.collider.tag == "Shootable Object")
                    {
                        objectPooler.SpawnFromObjectPool("Bullet Hole", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                    }
                    else if (hit.collider.tag == "Floor")
                    {
                        objectPooler.SpawnFromObjectPool("Bullet Impact Ground", hit.point + (hit.normal * 0.025f), Quaternion.LookRotation(hit.normal));
                    }
                }
            }
            else
            {
                // If the bullet hits nothing it still has a direction to fire
                firePosition.LookAt(mainCameraHead.position + (mainCameraHead.forward * 50f));
            }

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);

            objectPooler.SpawnFromObjectPool("Bullet", firePosition.position, firePosition.rotation);
            return;
        }
    }
}
