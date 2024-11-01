using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    private float speed = 3.0f;
    private float jumpSpeed = 10.0f;
    private float gravity = 10.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool isGrounded;

    //All the work done is reffered from Unity's official Documentation
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime * 1.5f;
        controller.Move(moveDirection * Time.deltaTime);
    }
}