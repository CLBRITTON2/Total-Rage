using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Backing fields
    public CharacterController playerController;
    public Transform mainCameraHead;
    public Transform ground;
    public LayerMask groundLayer;
    public Animator playerAnimator;

    private float playerWalkingSpeed = 10.0f, playerSprintSpeed = 17f;
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
    private float playerCrouchMovementSpeed = 6f;

    private bool playerIsSprinting = false, startSlidingTimer;
    private float currentSlideTimer, maxSlideTimer = 1f;
    private float playerSlideSpeed = 20f;
    #endregion

    // Start is called before the first frame update
    #region Start
    void Start()
    {
        playerBodyScale = playerBody.localScale;
        initialControllerHeight = playerController.height;
    }
    #endregion

    // Update is called once per frame
    #region Regular Update
    void Update()
    {
        PlayerFirstPersonView();
        InitializeJumpCheck();
        PlayerCrouching();
    }
    #endregion

    #region FixedUpdate
    private void FixedUpdate()
    {
        PlayerMovement();
        AddVelocityToPlayer();
        SlideCounter();
        if (initializeJump)
        {
            Jump();
        }
    }
    #endregion

    #region Method: Player Movement
    private void PlayerMovement()
    {
        // get the horizontal and vertical axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Combine the x and z axis, transform the Player object (inside the unity inspector "Transform")
        Vector3 move = x * transform.right + z * transform.forward;

        if (Input.GetKey(KeyCode.LeftShift) && !playerIsCrouching)
        {
            move = move * playerSprintSpeed * Time.deltaTime;
            playerIsSprinting = true;
        }
        else if (playerIsCrouching)
        {
            move = move * playerCrouchMovementSpeed * Time.deltaTime;
        }
        else
        {
            move = move * playerWalkingSpeed * Time.deltaTime;
            playerIsSprinting = false;
        }

        playerAnimator.SetFloat("PlayerSpeed", move.magnitude);

        playerController.Move(move);
    }
    #endregion
    #region Method: Player Crouching Main
    private void PlayerCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCrouching();
        }
        if (Input.GetKeyUp(KeyCode.C) || currentSlideTimer > maxSlideTimer)
        {
            StopCrouching();
        }
    }
    #endregion
    #region Method: Start Crouching
    private void StartCrouching()
    {
        playerBody.localScale = playerCrouchingScale;
        mainCameraHead.position -= new Vector3(0, 1f, 0);
        playerController.height /= 2;
        playerIsCrouching = true;

        if (playerIsSprinting && Input.GetKeyDown(KeyCode.C))
        {
            velocity = new Vector3(0f, 0f, 0f);
            velocity = Vector3.ProjectOnPlane(mainCameraHead.transform.forward, Vector3.up).normalized * playerSlideSpeed * Time.deltaTime;
            startSlidingTimer = true;
        }
    }
    #endregion
    #region Method: Stop Crouching
    private void StopCrouching()
    {
        currentSlideTimer = 0f;
        velocity = new Vector3(0f, 0f, 0f);
        startSlidingTimer = false;

        playerBody.localScale = playerBodyScale;
        mainCameraHead.position += new Vector3(0, 1f, 0);
        playerController.height = initialControllerHeight;
        playerIsCrouching = false;
    }
    #endregion
    #region Method: Player Velocity
    private void AddVelocityToPlayer()
    {
        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;
        if (playerController.isGrounded)
        {
            velocity.y = Physics.gravity.y * Time.deltaTime;
        }
        playerController.Move(velocity);
    }
    #endregion
    #region Method: Jump Check
    private void InitializeJumpCheck()
    {
        playerCanJump = Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;

        if (Input.GetButtonDown("Jump") && playerCanJump)
        {
            initializeJump = true;
        }
    }
    #endregion
    #region Method: Jump
    private void Jump()
    {
        // Height affected by gravity formula
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        playerController.Move(velocity);
        initializeJump = false;
    }
    #endregion
    #region Method: Player View
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
    #endregion
    private void SlideCounter()
    {
        if(startSlidingTimer)
        {
            currentSlideTimer += Time.deltaTime;
        }
    }   
}
