using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerMovementTest : MonoBehaviour
{
    //alternative script for player movement, suffers from thetiny offset i mention
    public float SPD;
    public int cellSize;
    public Vector3 destination, gridDestination;
    public bool isDestinationBlocked = false;
    public bool smoothTransition = false;
    public float transitionRotationSpeed = 500f;
    public AudioSource footSteps;
    public Tilemap floorTileMap;
    Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        StartUp();
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == destination && transform.rotation == Quaternion.Euler(targetRotation))
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (CheckDirection(transform.forward * cellSize))
                {
                    Debug.Log("forward");
                    destination += transform.forward * cellSize;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (CheckDirection(-transform.right * cellSize))
                {
                    Debug.Log("left");
                    destination -= transform.right * cellSize;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (CheckDirection(-transform.forward * cellSize))
                {
                    Debug.Log("back");
                    destination -= transform.forward * cellSize;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (CheckDirection(transform.right * cellSize))
                {
                    Debug.Log("right");
                    destination += transform.right * cellSize;
                }
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                targetRotation += Vector3.up * -90f;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                targetRotation += Vector3.up * 90f;
            }
            
        }
        

        Movement();
        Rotate();

    }

    public bool CheckDirection(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, cellSize))
        {
            if (hitInfo.collider.CompareTag("Obstacle"))
            {
                return false;
            }
        }
        return true;
    }

    public void StartUp()
    {
        footSteps = transform.GetComponent<AudioSource>();
        transform.position = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(transform.position));
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.y); //puts object to the center of cell except for the y position
        destination = transform.position;
        transform.rotation = Quaternion.identity;
        targetRotation = Vector3.zero;
    }

    public void Rotate()
    {
        if (targetRotation.y > 270f && targetRotation.y < 361f)
        {
            targetRotation.y = 0f;
        }
        if (targetRotation.y < 0f)
        {
            targetRotation.y = 270f;
        }

        if (transform.rotation != Quaternion.Euler(targetRotation))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
        }
    }


    public void Movement()
    {
        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * SPD);
        }
    }
}
