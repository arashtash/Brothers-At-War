using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator animator;
    int idleStateHash;
    int walkStateHash;
    int attackStateHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Cache the hash values for each state to improve performance
        idleStateHash = Animator.StringToHash("Base Layer.OneHand_Up_Idle");
        walkStateHash = Animator.StringToHash("Base Layer.OneHand_Up_Walk_F_InPlace");
        attackStateHash = Animator.StringToHash("Base Layer.OneHand_Up_Attack_1");
    }

    void Update()
    {
        // Get the current state info
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        bool fire = Input.GetButtonDown("Fire1");
        bool walk = Input.GetAxis("Vertical") != 0;

        // Transition to walk if the current state is idle and "W" is pressed
        if (walk && currentState.fullPathHash == idleStateHash)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else if (!walk && currentState.fullPathHash == walkStateHash)
        {
            animator.SetBool("isWalking", false);
        }

        // Transition to attack if the current state is idle and "Fire1" is pressed
        if (fire && currentState.fullPathHash == idleStateHash)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isWalking", false);
        }
    }
}
