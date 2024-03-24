using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float maxHP, currentHP, DMG, SPD;
    public int cellSize;
    public Vector3 destination;
    bool isMoving = false, isRotating = false;
    string direction;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                destination += transform.rotation * Vector3.forward * cellSize;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                destination += transform.rotation * Vector3.left * cellSize;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                destination += transform.rotation * Vector3.back * cellSize;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                destination += transform.rotation * Vector3.right * cellSize;
                isMoving = true;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!isRotating)
                {
                    StartCoroutine(Rotate(-90.0f));
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isRotating)
                {
                    StartCoroutine(Rotate(90.0f));
                }
            }
        }

        if (isMoving)
        {
            //make sure a movement input is there and the coroutine is not running already
            StartCoroutine(Movement(transform.position, destination));
        }

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


    public IEnumerator Rotate(float targetRotation)
    {
        //it rotates but not exactly on 90 degrees increment for some reason
        isRotating = true;
        float duration = 0.5f;
        float timeElapsed = 0;
        Quaternion originalRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(0, targetRotation, 0) * originalRotation;
        Debug.Log(finalRotation);

        while (transform.rotation != finalRotation)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, timeElapsed/duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        isRotating = false;
    }

    //thank you stack overflow for this code
    public IEnumerator Movement(Vector3 origin, Vector3 destination)
    {
        isMoving = true;
        float totalMovementTime = SPD; //the amount of time you want the movement to take
        float currentMovementTime = 0f;//The amount of time that has passed
        while (Vector3.Distance(transform.position, destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(origin, destination, currentMovementTime / totalMovementTime);
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
