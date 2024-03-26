using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class EnemyScript : MonoBehaviour
{
    public float maxHP, currentHP, DMG, SPD;
    public int cellSize;
    public Vector3 destination, gridDestination;
    public GameObject player;
    public bool isMoving = false, isRotating = false;
    public bool startMoving = false, startRotating = false;
    public bool smoothTransition = false;
    public float transitionRotationSpeed = 500f;
    public AudioSource footSteps;
    public Tilemap floorTileMap;
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
            if (!isMoving && !isRotating)
            {
                //if distance

                if (Mathf.Abs(player.transform.position.z - transform.position.z) > float.Epsilon)
                {
                    delta = (player.transform.position - transform.position).normalized;
                    cross = Vector3.Cross(delta, transform.forward);
                    if (cross == Vector3.zero)
                    {
                        Vector3 toTarget = (player.transform.position - transform.position).normalized;
                        //if the enemy is facing towards player
                        if (Vector3.Dot(toTarget, transform.forward) == 1)
                        {
                            Debug.Log("forward");
                            destination += transform.forward * cellSize;
                            gridDestination = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(destination));
                            destination = new Vector3(gridDestination.x, 1.5f, gridDestination.y);//get center of cell and make sure it only moves forward/backwards or left/right
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
        currentHP = maxHP;
        LookForPlayer();
        transform.position = floorTileMap.GetCellCenterLocal(Vector3Int.FloorToInt(transform.position));
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.y);
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
        footSteps.Play();
        isMoving = true;
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
        if (currentHP + modBy <= 0)
        {
            transform.DetachChildren();
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

    public void Pathing(Vector3Int cameFrom, Vector3Int current)
    {

    }

    public void LookForPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
