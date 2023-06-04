using UnityEngine;

/*
 * This class only rotates the npc in the direction of the player
 */
[RequireComponent(typeof(Rigidbody))]
public class RotateToFacePlayer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Transform playerTransform;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 targetDirection = playerTransform.position - transform.position;
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        rb.MoveRotation(targetRotation);
    }
}
