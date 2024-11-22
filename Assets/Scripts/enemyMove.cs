using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject[] waypoints;

    private int soundTimer = 120;
    private bool soundCanBePlayed = true;
    private int counter = 0;
    private Animation enemyAnimation;
    private AudioManager audiomanager;

    

    void Start()
    {
        enemyAnimation = GetComponent<Animation>();
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            if (counter < waypoints.Length)
            {
                Transform targetWaypoint = waypoints[counter].transform;
                float step = speed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

                Vector3 direction = targetWaypoint.position - transform.position;
                if (direction != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
                }

                if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
                {
                    counter++;
                }

                if (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
                {
                    enemyAnimation.CrossFade("run");
                }
                else
                {
                    enemyAnimation.CrossFade("idle");
                }
            }
            else
            {
                enemyAnimation.CrossFade("attack1");

                if (soundCanBePlayed)
                {
                    audiomanager.PlaySFX(audiomanager.goblinAttack);
                    soundTimer = 450;
                    soundCanBePlayed = false;
                }

                if (soundTimer > 0)
                {
                    soundTimer--;
                }
                else if (soundTimer <= 0)
                {
                    soundCanBePlayed = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("Waypoints not assigned correctly.");
        }
    }
}
