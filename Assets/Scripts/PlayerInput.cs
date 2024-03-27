using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode right = KeyCode.D;
    public KeyCode left = KeyCode.A;
    public KeyCode turnLeft = KeyCode.Q;
    public KeyCode turnRight = KeyCode.E;

    MovementController movementController;
    void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementController.smoothMovement == true)
        {
            #region Smooth Movement
            if (Input.GetKey(forward))
            {
                if (movementController.CheckDirection(Vector3.forward))
                {

                    movementController.MoveForward();
                }
            }
            if (Input.GetKey(backward))
            {
                if (movementController.CheckDirection(-Vector3.forward))
                {
                    movementController.MoveBackward();
                }
            }
            if (Input.GetKey(left))
            {
                if (movementController.CheckDirection(-Vector3.right))
                {
                    movementController.MoveLeft();
                }
            }
            if (Input.GetKey(right))
            {
                if (movementController.CheckDirection(Vector3.right))
                {
                    movementController.MoveRight();
                }
            }
            #endregion
        }
        else
        {
            #region Hard Movement
            if (Input.GetKeyUp(forward))
            {
                if (movementController.CheckDirection(Vector3.forward))
                {

                    movementController.MoveForward();
                }
            }
            if (Input.GetKeyUp(backward))
            {
                if (movementController.CheckDirection(-Vector3.forward))
                {
                    movementController.MoveBackward();
                }
            }
            if (Input.GetKeyUp(left))
            {
                if (movementController.CheckDirection(-Vector3.right))
                {
                    movementController.MoveLeft();
                }
            }
            if (Input.GetKeyUp(right))
            {
                if (movementController.CheckDirection(Vector3.right))
                {
                    movementController.MoveRight();
                }
            }
            #endregion
        }
        

        

        if (Input.GetKeyUp(turnLeft))
        {
            movementController.TurnLeft();
        }

        if (Input.GetKeyUp(turnRight))
        {
            movementController.TurnRight();
        }

    }
}
