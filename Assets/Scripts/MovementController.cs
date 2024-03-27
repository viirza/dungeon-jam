using TMPro;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public bool smoothTransition = false;
    public bool smoothMovement = false;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private LayerMask collisionMask = 0;
    [SerializeField] private float transitionRotationSpeed = 500f;

    Vector3 targetPosition;
    Vector3 targetRotation;


    bool AtRest
    {
        get
        {

            if ((Vector3.Distance(transform.position, targetPosition) < Mathf.Epsilon) &&
                (Vector3.Distance(transform.eulerAngles, targetRotation) < Mathf.Epsilon))
                return true;
            else
                return false;

        }
    }

    public bool CheckDirection(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, detectionRadius, collisionMask)) 
        {
            if (hitInfo.collider.CompareTag("Wall"))
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {

        if (targetRotation.y > 270f && targetRotation.y < 361f)
        {
            targetRotation.y = 0f;
        }


        if (targetRotation.y < 0f)
        {
            targetRotation.y = 270f;
        }

            if (!smoothTransition)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(targetRotation);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
        }
    }

    public void MoveForward()
    {
        if (AtRest)
        {
            targetPosition = transform.position + Vector3.forward;
        }
    }

    public void MoveBackward()
    {
        if (AtRest)
        {
            targetPosition = transform.position + -Vector3.forward;
        }
    }

    public void MoveLeft()
    {
        if (AtRest)
        {
            targetPosition = transform.position + -Vector3.right;
        }
    }

    public void MoveRight()
    {
        if (AtRest)
        {
            targetPosition = transform.position + Vector3.right;
        }
    }

    public void TurnRight() { if (AtRest) targetRotation += Vector3.up * 90f; }
    public void TurnLeft() { if (AtRest) targetRotation += Vector3.up * -90f; }

}
