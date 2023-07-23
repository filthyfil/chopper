using System.Collections;
using UnityEngine;

public class CharacterController3D : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float jumpForce = 8.0f;
    public float turnSmoothTime = 0.1f;
    public float groundDistance = 0.4f;
    public float jumpDelay = 0.2f; // The delay between jumps
    public Transform cameraT;
    public LayerMask groundMask;
    public Transform groundCheck;

    private float turnSmoothVelocity;
    private Vector3 direction;
    private Rigidbody rb;
    private float speed;
    private bool isJumping;
    private bool canJump = true; // Whether the player can currently jump


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (IsGrounded() && Input.GetButtonDown("Jump") && canJump)
        {
            isJumping = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Resets the velocity downwards from falling
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
    }

    void FixedUpdate()
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            speed = Input.GetKey(KeyCode.LeftShift) ? walkSpeed : runSpeed;

            rb.MovePosition(transform.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
        }

        // Reset isJumping if the character landed
        if (isJumping && IsGrounded())
        {
            isJumping = false;
        }
    }

    // Checks if the player is on the ground
    private bool IsGrounded()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded;
    }


    // Cooldown period after each jump
    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
    }
}
