
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
    [SerializeField] private float rotationSpeed;

    private Transform cameraTransform; // Required for calculating movementDirection
    private Rigidbody rb;

    private Vector3 movementDirection;
    private Vector3 rotationDirection;

    private void Awake()
    {
        cameraTransform = Camera.main.transform; // Only works if the camera is tagged as "MainCamera"
        rb = GetComponent<Rigidbody>();
    }

    // Handles all the player movement
    public void HandleMovement(Vector2 directionalInput)
    {
        // Directional movement happens even if there is no input (it sets the rb velocity to zero)
        MovePlayer(directionalInput);

        // Rotation only happens if there is an input
        if (directionalInput != Vector2.zero) RotatePlayer(directionalInput);
    }

    // Calculates the movement velocity and sets it to the rigidbody
    private void MovePlayer(Vector2 directionalInput)
    {
        // Camera based movement
        movementDirection = cameraTransform.forward * directionalInput.y;
        movementDirection += cameraTransform.right * directionalInput.x;

        movementDirection.Normalize();
        movementDirection *= walkSpeed; // Modify this later, to also allow sprinting
        movementDirection.y = rb.velocity.y; // Keeps the vertical velocity as calculated by the rigidbody

        rb.velocity = movementDirection;
    }

    // Rotates the player to match the movement direction
    private void RotatePlayer(Vector2 directionalInput)
    {
        Quaternion targetRotation;

        rotationDirection = cameraTransform.forward * directionalInput.y;
        rotationDirection += cameraTransform.right * directionalInput.x;

        rotationDirection.Normalize();
        rotationDirection.y = 0;

        targetRotation = Quaternion.LookRotation(rotationDirection);
        rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}