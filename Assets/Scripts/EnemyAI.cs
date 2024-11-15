using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject lane; // Reference to the path object
    public float speed = 2f; // Speed of the enemy
    public int damageToCastle = 10; // Amount of damage the enemy deals to the castle

    private Transform startPoint;   //Starting Point of the lane
    private Transform endPoint; //Ending point of the lane
    private Transform targetPoint;  //Target point in case path is not linear and has multiple waypoints

    // Start is called before the first frame update
    void Start()
    {
        // Get the starting and ending waypoints from the path GameObject
        startPoint = lane.transform.GetChild(0); // First child is the start point
        endPoint = lane.transform.GetChild(1);   // Second child is the end point

        // Set the initial target to the end point
        targetPoint = endPoint;

        // Move the enemy to the start point initially
        transform.position = startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the enemy towards the target point
        MoveTowardsTarget();
    }



    //This method moves an enemy toward a target point
    private void MoveTowardsTarget()
    {
        // Calculate the direction and move towards the target
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;   //Move towards the target

        // Check if the enemy is close to the target point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Deal damage to the castle and destroy the enemy
            DealDamageToCastle();
            Destroy(gameObject); // Destroy the enemy
        }
    }

    //This method deals damage to the castle by calling the take damage function in the CastleStats script
    private void DealDamageToCastle()
    {
        // Find the CastleHealth component on the castle GameObject
        CastleStats castleStats = FindObjectOfType<CastleStats>();

        //If the script for castleStats exist and is found
        if (castleStats != null)
        {
            //Deal damage to the castle
            castleStats.TakeDamage(damageToCastle);
        }
    }
}

