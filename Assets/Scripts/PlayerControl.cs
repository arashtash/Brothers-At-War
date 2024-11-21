using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    private float speed = 10.0f; // Base movement speed
    private float acceleration = 5.0f; // Smooth acceleration
    private float deceleration = 10.0f; // Smooth deceleration
    private float jumpSpeed = 7.5f;
    private float gravity = 10.0f;
    private Animator animator;
    public AudioSource source1;
    public AudioSource source2;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetMoveDirection = Vector3.zero; 
    private CharacterController controller;
    private bool isGrounded;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            // Get inputs for horizontal and vertical movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Create a target direction based on input
            Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            if (inputDirection != Vector3.zero)
            {
                // Smoothly interpolate movement direction
                targetMoveDirection = Vector3.Lerp(targetMoveDirection, inputDirection * speed, acceleration * Time.deltaTime);

                // Smoothly rotate the player towards the target direction
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);

                // Set walking animation
                animator.SetBool("isWalking", true);
            }
            else
            {
                // Instantly stop the movement when no input is detected
                targetMoveDirection = Vector3.zero;

                // Stop walking animation if movement stops
                animator.SetBool("isWalking", false);
            }

            // Jump logic
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply movement
        moveDirection = targetMoveDirection;
        moveDirection.y -= gravity * Time.deltaTime * 1.5f; // Apply gravity
        controller.Move(moveDirection * Time.deltaTime);

        // Attack logic
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
            source1.Play();

            Invoke("ResetAttack", 1.5f);
        }
    }

    private void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Goblin")){
            enemyHealth healthScript = collision.gameObject.GetComponent<enemyHealth>();
            if(healthScript != null){
                source2.Play();
                // healthScript.TakeDamage(10);
            }

            Destroy(gameObject);
        }
    }
}