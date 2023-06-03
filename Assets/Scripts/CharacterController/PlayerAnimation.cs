using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is responsible for interacting with the Animator and setting its parameters
 */
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    Animator animator;

    //Names of the animator parameters
    private const string speedParameter = "Speed";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleWalkingAnimation(Vector2 directionalInput, bool sprinting)
    {
        /* speedParameter == 0 => play idle animation
         * speedParameter == 1 => play walking animation
         * speedParameter == 2 => play running animation
         * 
         * This code is using 1.75 instead of 2 for running the animation because the blend between
         * running and walking looks more natural than the pure running animation. In case this
         * project gets new animations, you should also change this value.
         */

        float speedValue = directionalInput == Vector2.zero ? 0 : 1;
        if (sprinting) speedValue *= 1.75f;

        animator.SetFloat(speedParameter, speedValue);
    }

}
