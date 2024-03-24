using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float maxHP, currentHP, DMG, SPD;
    public int cellSize;
    public GameObject player;
    public Vector3 destination;
    bool isMoving = false, isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        LookForPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) != cellSize)
            {
                if (transform.position.z - player.transform.position.z > cellSize)
                {
                    destination += transform.rotation * Vector3.forward * cellSize;
                    MovementCheck(destination);
                }
                else if (transform.position.z - player.transform.position.z < cellSize)
                {
                    RotateCheck(90);
                }
                else if (transform.position.x - player.transform.position.x > cellSize)
                {
                    RotateCheck(-90);
                }
                else if (transform.position.x - player.transform.position.x < cellSize)
                {
                    RotateCheck(90);
                }
            }
            else
            {
                Attack();
            }
        }
    }


    public void UpdateHP(float modBy)
    {
        //make sure hp doesnt go over the max and kill the player when it reaches 0

        if (currentHP + modBy <= 0)
        {
            Destroy(gameObject);
        }
        else if (currentHP + modBy > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += modBy;
        }
    }

    public void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                PlayerScript ps = hit.collider.GetComponent<PlayerScript>();
                ps.UpdateHP(-DMG);
            }
        }
    }


    public void RotateCheck(float rotation)
    {
        if (!isRotating)
        {
            StartCoroutine(Rotate(rotation));
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
            transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        isRotating = false;
    }


    public void MovementCheck(Vector3 destination)
    {
        if (!isMoving)
        {
            StartCoroutine(Movement(transform.position, destination));
        }
    }

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


    public void LookForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
