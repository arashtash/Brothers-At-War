using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMove : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject[] waypoints;

    private int counter = 0;
    private Animation goblinAnimation;

    // Start is called before the first frame update
    void Start()
    {
        goblinAnimation = GetComponent<Animation>(); // Get the Animation component
    }

    // Update is called once per frame
    void Update()
    {
        if (counter < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[counter].transform;
            float step = speed * Time.deltaTime;

            // Move the goblin towards the waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            // Rotate the goblin to face the direction of movement
            Vector3 direction = targetWaypoint.position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
            }

            // Check if the goblin has reached the waypoint
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                counter++; // Move to the next waypoint
            }

            // Change animation based on movement
            if (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                goblinAnimation.CrossFade("run"); // Play running animation
            }
            else
            {
                goblinAnimation.CrossFade("idle"); // Play idle animation when stopping
            }
        }
        else
        {
            goblinAnimation.CrossFade("idle"); // Stop running when at the last waypoint
        }
    }
}
