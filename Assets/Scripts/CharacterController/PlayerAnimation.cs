/*
 * This class is responsible for interacting with the Animator and setting its parameters
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float speedValue = directionalInput == Vector2.zero ? 0 : 1;
        if (sprinting) speedValue *= 1.75f;

        animator.SetFloat(speedParameter, speedValue);
    }

}
