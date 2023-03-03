using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] bool UsePhysicsEngine;

    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Rigidbody body;
    public float followSpeed;
    public float rotationSpeed;

    // Tracks hands to controller movement and rotations, with an offset to accommodate for any
    // misaligned bones from the IK generation. Change the offsets in the editor to fix the tracking.
    // To increase the delay between the hand and controller, decrease 'Follow Speed' in the editor.
    private void Move()
    {
        // Update Position
        Vector3 positionWithOffset = Target.transform.TransformPoint(positionOffset);
        float distance = Vector3.Distance(positionWithOffset, transform.position);
        
        // If Physics Engine is being used, do not transform this unit using position.
        if (!UsePhysicsEngine)
            transform.position = positionWithOffset;

        // Update Rotation
        Quaternion rotationWithOffset = Target.transform.rotation * Quaternion.Euler(rotationOffset);
        transform.rotation = rotationWithOffset;
    }

    // Physics Engine Support
    void PhysMove()
    {
        // If Physics Engine is being used, transform this unit using velocity.
        if (UsePhysicsEngine)
        {
            Vector3 positionWithOffset = Target.transform.TransformPoint(positionOffset);
            float distance = Vector3.Distance(positionWithOffset, transform.position);
            body.velocity = (positionWithOffset - transform.position).normalized * (followSpeed * distance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Target.transform.position;
        positionOffset = Target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        PhysMove();
    }
}
