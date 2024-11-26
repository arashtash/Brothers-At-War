using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject[] waypoints;

    private int counter = 0;
    private Animation enemyAnimation;

    private CastleStats castleStats; //link to the Castle stats script
    private float attackTimer = 0f; //timer to control damage intervals based on the animation duration

    //This method is executed when enemy is first created
    void Start()
    {
        enemyAnimation = GetComponent<Animation>(); //Find and get the animation component

        // Find the CastleStats component
        castleStats = FindObjectOfType<CastleStats>();
        if (castleStats == null)    //If no castle is found
        {
            Debug.LogError("CastleStats script not found in the scene!");   //Report it
        }
    }

    //This method is executed at each frame
    void Update()
    {
        //If there are waypoints, and  the length of the way points are valid
        if (waypoints != null && waypoints.Length > 0)
        {
            if (counter < waypoints.Length) //If it hasn't reached the last way point
            {
                MoveToWaypoint();   //Move toward the waypoint
            }
            else
            {
                AttackCastle(); //start attacking the castle
            }
        }
        else //If waypoints are invalid
        {
            Debug.LogWarning("Waypoints not assigned correctly.");  //Report it
        }
    }

    //This method moves the player toward a way point
    private void MoveToWaypoint()
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

    //This method handles enemy attacking the castle
    private void AttackCastle()
    {   
        //Activate attack animation
        enemyAnimation.CrossFade("attack1");


        // Handle damage
        float animationDuration = enemyAnimation["attack1"].length; //Get animation duration
        attackTimer -= Time.deltaTime; //Reduce timer by frame time

        if (attackTimer <= 0f) //When timer starts, start dealing damage to the castle
        {
            DealDamageToCastle();   //Damage the castle
            attackTimer = animationDuration; //Reset timer to animation duration
        }
    }

    //This method handles the enemy dealing damage to the castle
    private void DealDamageToCastle()
    {
        //IF the castle script exists and is found
        if (castleStats != null)
        {
            Debug.Log("Dealing 10 damage to the castle.");
            castleStats.TakeDamage(10); //Inflict damage
        }
        else //If no castle stats script is found
        {
            Debug.LogError("CastleStats is null. Cannot deal damage!"); //Report it
        }
    }
}
