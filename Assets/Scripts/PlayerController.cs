using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Mouse sensitivity")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [Header("Movement speed")]
    [SerializeField] private float speed;
    [Header("Jump force")]
    [SerializeField] private float jumpMultiplier;
    [Header("Max distance")]
    [SerializeField] private float maxDistance;
    [Space]
    [SerializeField] private GameObject orientation;
    [SerializeField] private LayerMask groundLayer;

    private float horizontalInput;
    private float verticalInput;
    private float mouseX;
    private float mouseY;
    private float pitch = 0;
    private bool isGrounded;
    private Ray ray;

    private Rigidbody rb;
    


    // Start is called before the first frame update
    void Start()
    {
        // get components
        rb = GetComponent<Rigidbody>();

        // initialize 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate(){
        move();
    }

    // Update is called once per frame
    void Update()
    {
        // check ray
        ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, groundLayer)) {
            // when distance is close from ground
            if (hit.distance < 1.2) {
                jump();
            }
        }
        rotatePlayerAndCam();
    }

    void move()
    {
        // change player pos by orientation
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 playerInput = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movement = transform.TransformDirection(playerInput) * speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    void rotatePlayerAndCam()
    {
        // change player's facing direction
        mouseX = Input.GetAxis("Mouse X");
        mouseY = -Input.GetAxis("Mouse Y"); // Y axis is inverted

        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * sensX); // rotate player body
        pitch += mouseY * Time.deltaTime * sensY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        orientation.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void jump()
    {
        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpMultiplier, rb.velocity.z);
        }
    }
}
