using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Game objects
    public CharacterController controller;
    public Transform cam;
    public GameObject player;
    public GameObject playerDetails;

    //ground stuff
    public Transform groundCheck;
    public float groundDistance = 0.39f;
    public LayerMask groundMask;

    //smoothing
    public float playerTurnSmoothTime;
    float turnSmoothVelocity;

    //speeds and multipliers
    public float horizontalInput;
    public float verticalInput;
    public float playerSpeed = 10f;
    public Vector3 playerDirection;

    //jumping and gravity behavior
    public float jumpHeight = 1f;
    public float gravity = -9.81f;
    Vector3 velocity;
    public bool isJumping;
    public bool isMovingJumping;
    public int jumpMod;
    public float jumpButtonGracePeriod = 0.05f;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    //game bools
    public bool isDead;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -4f;
            isJumping = false;
            isMovingJumping = false;
            jumpMod = 0;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal"); //player input
        verticalInput = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;//calcualte intended movement

        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (!controller.isGrounded && playerDirection.magnitude >= 0.1f && (isMovingJumping || !isJumping) || jumpMod == 1)
        {
            Vector3 jumpDirection = transform.TransformDirection(Vector3.forward);
            jumpMod = 1;
            controller.Move(jumpDirection.normalized * playerSpeed * Time.deltaTime * jumpMod);
        }

        if ((playerDirection.magnitude >= 0.1f) || Input.GetKeyDown(KeyCode.Space)) //player movement WASD
        {
            if ((Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod) &&
            (Time.time - lastGroundedTime <= jumpButtonGracePeriod)) //player movement jump
            {
                if (playerDirection.magnitude >= 0.1f)
                {
                    isMovingJumping = true;
                }
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
             if (controller.isGrounded)
            {
                float targetAngle = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
            }
        }

        if (playerDirection.magnitude == 0 && jumpMod == 0) //have the same y rotation value as camera when not moving
        {
            float targetCamAngle = cam.eulerAngles.y;
            float camAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetCamAngle, ref turnSmoothVelocity, playerTurnSmoothTime);
            player.transform.rotation = Quaternion.Euler(0f, camAngle, 0f);
        }

        velocity.y += gravity * Time.deltaTime; //gravity
        controller.Move(velocity * Time.deltaTime);
    }


}
