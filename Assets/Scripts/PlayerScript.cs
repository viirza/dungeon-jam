using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float maxHP, currentHP, DMG, SPD;
    public int cellSize;
    public Vector3 destination;
    public bool isMoving = false, isRotating = false;
    public bool startMoving = false, startRotating = false;
    public bool isDestinationBlocked = false;
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
                if (!MovementCheck(destination + transform.forward * cellSize))
                {
                    destination += transform.forward * cellSize;
                    startMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (!MovementCheck(destination - transform.right * cellSize))
                {
                    destination -= transform.right * cellSize;
                    startMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (!MovementCheck(destination - transform.forward * cellSize))
                {
                    destination -= transform.forward * cellSize;
                    startMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!MovementCheck(destination + transform.right * cellSize))
                {
                    destination += transform.right * cellSize;
                    startMoving = true;
                }
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
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    EnemyScript es = hit.collider.GetComponent<EnemyScript>();
                    es.UpdateHP(-DMG);
                }
            }
        }
    }

    //checks if the destination is blocked by an obstacle or enemy
    public bool MovementCheck(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 1))
        {
            if (hit.collider.gameObject.tag == "Obstacle" || hit.collider.gameObject.tag == "Enemy")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void StartUp()
    {
        currentHP = maxHP;
        destination = transform.position;
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


    public void UpdateHP(float modBy)
    {
        //make sure hp doesnt go over the max and kill the player when it reaches 0
        if(currentHP + modBy <= 0)
        {
            transform.DetachChildren();
            Destroy(gameObject);
        }
        else if(currentHP + modBy > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += modBy;
        }
    }
}
// Testing for any changes in Github