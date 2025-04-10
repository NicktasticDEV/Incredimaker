using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSpeed : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the BPM parameter from the Animator
        float bpm = animator.GetFloat("bpm");

        // Calculate the animation speed multiplier
        // Base BPM is 120, so the speed multiplier is bpm / 120
        float speedMultiplier = bpm / 120f;

        // Set the Animator speed dynamically
        if (animator.GetBool("isPaused") == false)
        {
            // If the metronome is playing, set the speed multiplier
            animator.speed = speedMultiplier;
        }
        else
        {
            // If the metronome is not playing, set the speed to 0
            animator.speed = 0f;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the Animator speed to 1 when exiting the state
        animator.speed = 1f;
    }
}