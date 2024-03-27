using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float SPD;
    public int cellSize;
    public Vector3 destination;
    public GameObject player;
    public bool isMoving = false, isRotating = false;
    public bool startMoving = false, startRotating = false;
    public bool smoothTransition = false;
    public float transitionRotationSpeed = 500f;
    Vector3 targetRotation, delta, cross;

    // Start is called before the first frame update
    void Start()
    {
        StartUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //if enemy is not in range go towards
            if (Mathf.Abs(Vector3.Distance(player.transform.position, transform.position)) > cellSize)
            {
                if (!isMoving && !isRotating)
                {
                    delta = (player.transform.position - transform.position).normalized;
                    cross = Vector3.Cross(delta, transform.forward);
                    if (cross == Vector3.zero)
                    {
                        Vector3 toTarget = (player.transform.position - transform.position).normalized;

                        if (Vector3.Dot(toTarget, transform.forward) > 0)
                        {
                            Debug.Log("forward");
                            destination += transform.forward * cellSize;
                            startMoving = true;
                        }
                        else
                        {
                            Debug.Log("behind");
                            targetRotation += Vector3.up * 90f;
                            startRotating = true;
                        }
                    }
                    else if (cross.y > 0)
                    {
                        Debug.Log("left");
                        targetRotation += Vector3.up * -90f;
                        startRotating = true;
                    }
                    else
                    {
                        Debug.Log("right");
                        targetRotation += Vector3.up * 90f;
                        startRotating = true;
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
                }
            }
            else
            {
                Debug.Log("attack");
                Attack();
            }
        }
    }


    public void StartUp()
    {
        LookForPlayer();
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



    public void Attack()
    {
        AttackComponent attackComponent = gameObject.GetComponent<AttackComponent>();
        attackComponent.Attack();
    }

    public void LookForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
