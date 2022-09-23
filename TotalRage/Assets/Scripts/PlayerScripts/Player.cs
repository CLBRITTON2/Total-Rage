using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Backing fields
    public CharacterController PlayerController;
    public Transform MainCameraHead;
    public Transform Ground;
    public LayerMask GroundLayer;
    public Animator PlayerAnimator;

    private float _playerWalkingSpeed = 10.0f, _playerSprintSpeed = 17f;
    public float MouseSensitivity = 700f;
    private float _cameraVerticalRotation;

    public Vector3 Velocity;
    public float GravityModifier;

    public float JumpHeight = 10f;
    private bool _playerCanJump;
    private bool _initializeJump;
    public float GroundDistance = 0.5f;

    private Vector3 _playerCrouchingScale = new Vector3(1f, 0.6f, 1f);
    private Vector3 _playerBodyScale;
    public Transform PlayerBody;
    private float _initialControllerHeight;
    private bool _playerIsCrouching;
    private float _playerCrouchMovementSpeed = 6f;

    private bool _playerIsSprinting = false, _startSlidingTimer;
    private float _currentSlideTimer, _maxSlideTimer = 1f;
    private float _playerSlideSpeed = 20f;
    #endregion

    // Start is called before the first frame update
    #region Start
    void Start()
    {
        _playerBodyScale = PlayerBody.localScale;
        _initialControllerHeight = PlayerController.height;
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
        if (_initializeJump)
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

        if (Input.GetKey(KeyCode.LeftShift) && !_playerIsCrouching)
        {
            move = move * _playerSprintSpeed * Time.deltaTime;
            _playerIsSprinting = true;
        }
        else if (_playerIsCrouching)
        {
            move = move * _playerCrouchMovementSpeed * Time.deltaTime;
        }
        else
        {
            move = move * _playerWalkingSpeed * Time.deltaTime;
            _playerIsSprinting = false;
        }

        PlayerAnimator.SetFloat("PlayerSpeed", move.magnitude);

        PlayerController.Move(move);
    }
    #endregion
    #region Method: Player Crouching Main
    private void PlayerCrouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCrouching();
        }
        if (Input.GetKeyUp(KeyCode.C) || _currentSlideTimer > _maxSlideTimer)
        {
            StopCrouching();
        }
    }
    #endregion
    #region Method: Start Crouching
    private void StartCrouching()
    {
        PlayerBody.localScale = _playerCrouchingScale;
        MainCameraHead.position -= new Vector3(0, 1f, 0);
        PlayerController.height /= 2;
        _playerIsCrouching = true;

        if (_playerIsSprinting && Input.GetKeyDown(KeyCode.C))
        {
            Velocity = Vector3.ProjectOnPlane(MainCameraHead.transform.forward, Vector3.up).normalized * _playerSlideSpeed * Time.deltaTime;
            _startSlidingTimer = true;
        }
    }
    #endregion
    #region Method: Stop Crouching
    private void StopCrouching()
    {
        _currentSlideTimer = 0f;
        Velocity = new Vector3(0f, 0f, 0f);
        _startSlidingTimer = false;

        PlayerBody.localScale = _playerBodyScale;
        MainCameraHead.position += new Vector3(0, 1f, 0);
        PlayerController.height = _initialControllerHeight;
        _playerIsCrouching = false;
    }
    #endregion
    #region Method: Player Velocity
    private void AddVelocityToPlayer()
    {
        Velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * GravityModifier;
        if (PlayerController.isGrounded)
        {
            Velocity.y = Physics.gravity.y * Time.deltaTime;
        }
        PlayerController.Move(Velocity);
    }
    #endregion
    #region Method: Jump Check
    private void InitializeJumpCheck()
    {
        _playerCanJump = Physics.OverlapSphere(Ground.position, GroundDistance, GroundLayer).Length > 0;

        if (Input.GetButtonDown("Jump") && _playerCanJump)
        {
            _initializeJump = true;
        }
    }
    #endregion
    #region Method: Jump
    private void Jump()
    {
        // Height affected by gravity formula
        Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        PlayerController.Move(Velocity);
        _initializeJump = false;
    }
    #endregion
    #region Method: Player View
    private void PlayerFirstPersonView()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * MouseSensitivity * Time.deltaTime;

        // Disables look inversion. If you move your mouse up you will look up
        _cameraVerticalRotation -= mouseY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);

        // Apply local rotation to only rotate the camera
        // Quaternion is used for rotation
        // Euler returns vector 3 converted into rotations
        MainCameraHead.localRotation = Quaternion.Euler(_cameraVerticalRotation, 0f, 0f);
    }
    #endregion
    private void SlideCounter()
    {
        if (_startSlidingTimer)
        {
            _currentSlideTimer += Time.deltaTime;
        }
    }
    private void PlayFirstStep()
    {
        AudioManager.Instance.PlaySound("PlayerFirstStep");
    }
    private void PlaySecondStep()
    {
        AudioManager.Instance.PlaySound("PlayerSecondStep");
    }
}
