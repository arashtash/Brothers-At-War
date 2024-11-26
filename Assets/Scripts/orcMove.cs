using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcMove : MonoBehaviour
{
    public float speed = 3.0f;
    public GameObject[] waypoints;


    private int counter = 0;
    private Animator animator;


    private CastleStats castleStats; //Link to the CastleStats script
    private float attackTimer = 0f; //timer for controlling damage intervals

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animation component

        // Find the CastleStats component
        castleStats = FindObjectOfType<CastleStats>();
        if (castleStats == null) //If it doesn't exist
        {
            Debug.LogError("CastleStats script not found in the scene!");   //Report it!
        }
        else //If castle script was found
        {
            Debug.Log("Successfully found CastleStats script."); //Report it
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            if (counter < waypoints.Length)
            {
                MoveToWaypoint();
            }
            else
            {
                AttackCastle(); // Start attacking the castle
            }
        }
        else
        {
            Debug.LogWarning("Waypoints not assigned correctly.");
        }
    }

    private void MoveToWaypoint()
    {
        Transform targetWaypoint = waypoints[counter].transform;
        float step = speed * Time.deltaTime;

        // Move the orc towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Rotate the orc to face the direction of movement
        Vector3 direction = targetWaypoint.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        }

        // Check if the orc has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            counter++; // Move to the next waypoint
        }
    }

    private void AttackCastle()
    {
        animator.SetBool("attack", true); // Trigger the attack animation

        //IDK WHAT IS THE NAME OF THE ORC'S ATTACK ANIMATION. I DON'T KNOW HOW ANIMATOR OBJECTS WORK
        //I NEED TO KNOW THAT TO DETERMINE THE LENGTH OF EACH "HIT" TO THE CASTLE TO DEAL DAMAGE TO IT
        // Handle damage timing
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack"))
        {
            float animationDuration = stateInfo.length; //Get the animation duration
            attackTimer -= Time.deltaTime; //Decrement timer

            if (attackTimer <= 0f) //Timer has elapsed, deal damage
            {
                DealDamageToCastle();
                attackTimer = animationDuration; //reset timer to animation duration
            }
        }
    }

    //This method deals damage to the castle
    private void DealDamageToCastle()
    {
        if (castleStats != null)    //If castle exist
        {
            castleStats.TakeDamage(10); // Inflict 10 damage
        }
        else //Otherwise if castle cannot be found
        {
            Debug.LogError("CastleStats is null. Cannot deal damage!"); //Report it
        }
    }
}