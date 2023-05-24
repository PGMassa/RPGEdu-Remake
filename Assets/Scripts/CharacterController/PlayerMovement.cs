
/*
 * This class is responsible for all physical movement of the player, including it's rotation.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed;

    private Transform cameraTransform; // Required for calculating movementDirection
    private Rigidbody rb;

    private Vector3 movementDirection;

    private void Awake()
    {
        cameraTransform = Camera.main.transform; // Only works if the camera is tagged as "MainCamera"
        rb = GetComponent<Rigidbody>();
    }

    // Calculates the apropriate movement direction and sets a new rigidbody velocity
    public void MovePlayer(Vector2 directionalInput)
    {
        // Camera based movement
        movementDirection = cameraTransform.forward * directionalInput.y;
        movementDirection += cameraTransform.right * directionalInput.x;

        movementDirection.Normalize();
        movementDirection *= walkSpeed; // Modify this later, to also allow sprinting
        movementDirection.y = rb.velocity.y; // Keeps the vertical velocity as calculated by the rigidbody

        rb.velocity = movementDirection;
    }
}
