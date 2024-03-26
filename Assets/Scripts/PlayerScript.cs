using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    public float maxHP, currentHP, DMG, SPD;
    public int cellSize;
    public Vector3 destination, gridDestination;
    public bool isMoving = false, isRotating = false;
    public bool startMoving = false, startRotating = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        //only accept input if player is currently not moving or rotating
        if (!isMoving && !isRotating)
        {
            //movement check is disabled and somethings the destination is diagonal for some reason
            if (Input.GetKey(KeyCode.W))
            {
                if (!MovementCheck(destination + transform.forward * cellSize))
                {
                    Debug.Log("forward");
                    destination += transform.forward * cellSize;
                    gridDestination = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(destination));
                    destination = new Vector3(gridDestination.x, 1.5f, gridDestination.y);//get center of cell and make sure it only moves forward/backwards or left/right
                    startMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (!MovementCheck(destination - transform.right * cellSize))
                {
                    Debug.Log("left");
                    destination -= transform.right * cellSize;
                    gridDestination = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(destination));
                    destination = new Vector3(gridDestination.x, 1.5f, gridDestination.y);
                    startMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (!MovementCheck(destination - transform.forward * cellSize))
                {
                    Debug.Log("back");
                    destination -= transform.forward * cellSize;
                    gridDestination = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(destination));
                    destination = new Vector3(gridDestination.x, 1.5f, gridDestination.y);
                    startMoving = true;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!MovementCheck(destination + transform.right * cellSize))
                {
                    Debug.Log("right");
                    destination += transform.right * cellSize;
                    gridDestination = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(destination));
                    destination = new Vector3(gridDestination.x, 1.5f, gridDestination.y);
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
                return false;
                //return true;
                //disabled the true check for now since it's kinda broken
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
        footSteps = transform.GetComponent<AudioSource>();
        transform.position = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(transform.position));
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.y); //puts object to the center of cell except for the y position
        destination = transform.position;
    }

    public IEnumerator Rotate()
    {
        isRotating = true;
        footSteps.Play();
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
        footSteps.Stop();
        isRotating = false;
    }


    public IEnumerator Movement()
    {
        isMoving = true;
        footSteps.Play();
        while (Vector3.Distance(transform.position, destination) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * SPD);
            yield return null;
        }
        footSteps.Stop();
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