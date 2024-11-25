using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform player; // Assign your player Transform in the Inspector
    public float rotationSpeed = 100f;
    public Vector3 offset; // Offset to keep the camera at a specific position relative to the player

    void Start()
    {
        // Calculate the initial offset based on the current positions of the camera and the player
        offset = transform.position - player.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            RotateCamera(-1);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            RotateCamera(1);
        }
    }

    void RotateCamera(float direction)
    {
        // Calculate the rotation angle
        float angle = direction * rotationSpeed * Time.deltaTime;

        // Rotate the offset around the player
        offset = Quaternion.AngleAxis(angle, Vector3.up) * offset;

        // Update the camera position to maintain the offset
        transform.position = player.position + offset;

        // Ensure the camera always looks at the player
        transform.LookAt(player);
    }
}
