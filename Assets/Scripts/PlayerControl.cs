using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    private float speed = 10.0f;
    private float rotationSpeed = 10.0f;
    private float jumpSpeed = 14.5f;
    private float gravity = 10.0f;
    public float power = 1.0f;
    private Animator animator;
    public AudioSource source1;
    public AudioSource source2;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool isGrounded;

    private bool isShift;

    void Start()
    {
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        isShift = false;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            float horizontalInput = 0 - Input.GetAxisRaw("Horizontal"); 
            float verticalInput = 0 - Input.GetAxisRaw("Vertical");

            Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            if (inputDirection != Vector3.zero)
            {        
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    isShift = true;
                }

                else if(Input.GetKeyUp(KeyCode.LeftShift)){
                    isShift = false;
                }


                if (isShift)
                {
                    Debug.Log("Sprinting");
                    moveDirection = inputDirection * (2.0f * speed);
                }
                else
                {
                    Debug.Log("Not sprinting");
                    moveDirection = inputDirection * speed;
                }
        
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                animator.SetBool("isWalking", true);
            }
            else
            {
                moveDirection = Vector3.zero;
                animator.SetBool("isWalking", false);
            }

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goblin") || collision.gameObject.CompareTag("Orc"))
        {
            enemyHealth healthScript = collision.gameObject.GetComponent<enemyHealth>();
            if (healthScript != null)
            {
                source2.Play();
            }

            healthScript.TakeDamage(power);
        }
    }
}