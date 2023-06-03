using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class only rotates the npc in the direction of the player
 */
[RequireComponent(typeof(Rigidbody))]
public class RotationToTarget : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 targetDirection = targetTransform.position - transform.position;
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.MoveRotation(targetRotation);
    }
}
