using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float SPD, DMG;
    public int cellSize;
    public Vector3 destination;
    public bool isMoving = false, isRotating = false;
    public bool startMoving = false, startRotating = false;
    public bool smoothTransition = false;
    public float transitionRotationSpeed = 500f;
    Vector3 targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        StartUp();
    }

    // Update is called once per frame
    void Update()
    {
        //only accept input if player is currently not moving or rotating
        if (!isMoving && !isRotating)
        {
            if (Input.GetKey(KeyCode.W))
            {
                destination += transform.forward * cellSize;
                startMoving = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                destination -= transform.right * cellSize;
                startMoving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                destination -= transform.forward * cellSize;
                startMoving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                destination += transform.right * cellSize;
                startMoving = true;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                targetRotation += Vector3.up * -90f;
                startRotating = true;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                targetRotation += Vector3.up * 90f;
                startRotating = true;
            }
        }

        //making sure that the player is only moving or rotating
        if (startMoving && !isMoving)
        {
            startMoving = false;
            StartCoroutine(Movement());
        }
        else if (startRotating && !isRotating)
        {
            startRotating = false;
            StartCoroutine(Rotate());
        }


        //attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackComponent attackComponent = GetComponent<AttackComponent>();
            attackComponent.Attack();
        }
    }

    public void StartUp()
    {
        destination = Vector3Int.RoundToInt(transform.position);
    }

    public IEnumerator Rotate()
    {
        isRotating = true;
        if (targetRotation.y > 270f && targetRotation.y < 361f)
        {
            targetRotation.y = 0f;
        }
        if (targetRotation.y < 0f)
        {
            targetRotation.y = 270f;
        }

        while (transform.eulerAngles != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
            yield return null;
        }
        isRotating = false;
    }


    public IEnumerator Movement()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, destination) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * SPD);
            yield return null;
        }
        isMoving = false;
    }

}