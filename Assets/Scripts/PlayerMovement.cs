using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20.0f;
    public GameObject character;

    new Animation animation;
    public Vector3 jump;
    public Vector3 jumpReset;
    public float jumpForce = 5.0f;
    public float boost = 1.0f;
    public float sprintModifier = 1.0f;
    public int curRotation = 0;

    public bool isEPressed = false;
    public bool isQPressed = false;
    public bool isGrounded;
    public bool groundAnimation;
    public bool jumpPlayed;
    public int jumpCount;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animation = GetComponent<Animation>();
        
        jump = new Vector3(0.0f, 5.0f, 0.0f);
        jumpCount = 0;
    }

    void OnCollisionEnter(){
        Debug.Log("Jump Refreshed");
        jumpCount = 0;
        isGrounded = true;
        boost = 1.0f;

        groundAnimation = true;
        jumpPlayed = false;
    }


    void OnCollisionExit(){
        groundAnimation = false;
    }
    // Update is called once per frame
    void Update()
    {
        //Handles if the character should be sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            Debug.Log("LeftShift is Pressed");
            sprintModifier = 1.6f;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)){
            Debug.Log("LeftShift is No Longer Pressed");
            sprintModifier = 1.0f;
        }

        //Handles the jump logic
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){

            animation.Play("jump");
            if (jumpCount == 0){
                Debug.Log("First Jump");
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            }

            else if (jumpCount == 1){
                Debug.Log("Double Jump");
                rb.velocity = new Vector3(0.0f,0.0f,0.0f);
                rb.AddForce((jump * jumpForce) / 1.3f, ForceMode.Impulse);
                boost = 0.8f;
            }
            jumpCount++;
            if (jumpCount > 1){
                isGrounded = false;
            }
        }

        //Find out the speed the character should be moving
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed * boost * sprintModifier;

        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed * boost * sprintModifier;

        //Handles the rotation of the character
        if (Input.GetKeyDown(KeyCode.E)){
			isEPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.E)){
			isEPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.Q)){
			isQPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Q)){
			isQPressed = false;
        }
		
        if(isEPressed){
            transform.localRotation =  Quaternion.Euler(new Vector3(0, curRotation++, 0));
        }
        else if (isQPressed){
			transform.localRotation = Quaternion.Euler(new Vector3(0, curRotation--, 0));
        }

        //Handles the animation logic
        if((x != 0.0f || z != 0.0f) && sprintModifier != 1 && groundAnimation){
            animation.Play("run");
        }

        else if((x != 0.0f || z != 0.0f) && sprintModifier == 1 && groundAnimation){
            animation.Play("walk");
        }

        // else if(!(groundAnimation) && !jumpPlayed){
        //     animation.Play("jump");
        //     jumpPlayed = true;
        // }

        else if (groundAnimation){
            animation.Play("idle");
            }

        transform.Translate(x, 0, z);
    }
}
