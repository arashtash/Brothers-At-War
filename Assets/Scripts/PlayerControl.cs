using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    private float speed = 10.0f;
    private float jumpSpeed = 7.5f;
    private float gravity = 10.0f;
    Animator animator;

    private Vector3 moveDirection = Vector3.zero;
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
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Determine the target direction based on input
            Vector3 targetDirection = Vector3.zero;

            if (verticalInput > 0)
            {
                targetDirection = Vector3.forward;
            }
            else if (verticalInput < 0)
            {
                targetDirection = Vector3.back;
            }
            else if (horizontalInput > 0)
            {
                targetDirection = Vector3.right;
            }
            else if (horizontalInput < 0)
            {
                targetDirection = Vector3.left;
            }

            // If there is input, rotate and move in the target direction
            if (targetDirection != Vector3.zero)
            {
                // Rotate the player towards the target direction
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);

                // Move in the target direction
                moveDirection = targetDirection * speed;
                animator.SetBool("isWalking", true);
            }
            else
            {
                moveDirection = Vector3.zero;
                animator.SetBool("isWalking", false);
            }

            // Jump logic
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Attack logic
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
            Invoke("ResetAttack", 1.5f);
        }

        moveDirection.y -= gravity * Time.deltaTime * 1.5f;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
    }
}