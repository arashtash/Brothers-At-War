using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public float speed = 5.0f;
    public GameObject[] waypoints;

    //animation variables
    private Animation animation;

    //sound variables
    private int soundTimer = 120;
    bool soundCanBePlayed = true;
    private int counter = 0;
    AudioManager audiomanager;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>(); // Get the Animation component
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // Get the audio manager component
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
                animation.CrossFade("run"); // Play running animation
            }
            else
            {
                animation.CrossFade("idle"); // Play idle animation when stopping
            }
        }
        else
        {
            animation.CrossFade("attack1"); // last waypoint is castle, attack the castle

            // Play sound if sound can be played
            if (soundCanBePlayed)
            {
                //play attack sound when hitting castle
                if (gameObject.tag == "Goblin")
                {
                    audiomanager.PlaySFX(audiomanager.goblinAttack);
                }
                soundTimer = 450;
                soundCanBePlayed = false;
            }

            // decrement timer until timer can be played
            if (soundTimer > 0)
            {
                soundTimer--;
            } else if (soundTimer <= 0)
            {
                soundCanBePlayed = true;
            }
        }
    }
}
