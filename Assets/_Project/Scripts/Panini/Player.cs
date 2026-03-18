using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isGrounded;
    public bool isSprinting;

    public Transform cam;
    public Rigidbody rb;
    public World world;

    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = -9.8f;

    public float playerWidth = 0.15f;
    public float boundsTolerance = 0.1f;

    public float mouseSensibility = 1f;

    private float horizontal;
    private float vertical;
    private float mouseHorizontal;
    private float mouseVertical;
    private Vector3 velocity;
    private float verticalMomemtum = 0;
    private bool jumpRequest;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
        if (jumpRequest)
            Jump();

        transform.Rotate(Vector3.up * mouseHorizontal * mouseSensibility);
        cam.Rotate(Vector3.right * -mouseVertical * mouseSensibility);
        // transform.Translate(velocity, Space.World);
        rb.AddForce(velocity * 50f, ForceMode.Force);
        float velocityX = Mathf.Clamp(rb.linearVelocity.x, 0, sprintSpeed);
        float velocityZ = Mathf.Clamp(rb.linearVelocity.z, 0, sprintSpeed);
        // rb.linearVelocity = new Vector3(velocityX, rb.linearVelocity.y, velocityZ);
    }

    private void Update()
    {
        GetPlayerInputs();
    }

    void Jump()
    {
        //verticalMomemtum = jumpForce;
        //isGrounded = false;
        //jumpRequest = false;
    }

    private void CalculateVelocity()
    {
        // Affect vertical momentum with gravity.
        //if (verticalMomemtum > gravity)
        //    verticalMomemtum += Time.fixedDeltaTime * gravity;

        // if we're sprinting, use the sprint multiplier.
        if (isSprinting)
            velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * sprintSpeed;
        else
            velocity = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime * walkSpeed;

        // Apply vertical momentum (falling/jumping).
        velocity += Vector3.up * verticalMomemtum * Time.fixedDeltaTime;
    }

    private void GetPlayerInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Sprint"))
            isSprinting = true;
        if (Input.GetButtonUp("Sprint"))
            isSprinting = false;

        if (isGrounded && Input.GetButtonDown("Jump"))
            jumpRequest = true;
    }
}
