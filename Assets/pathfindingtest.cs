using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class pathfindingtest : MonoBehaviour
{
    //test script trying out A*
    public GameObject target;
    public bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int zDir = 0;

        if (Mathf.Abs(target.transform.position.x - transform.position.x) < float.Epsilon)
            zDir = target.transform.position.z > transform.position.z ? 1 : -1;
        else
            xDir = target.transform.position.x > transform.position.x ? 1 : -1;

        if (!isMoving)
        {
            StartCoroutine(Movement(transform.position + new Vector3(xDir, 0, zDir)));
        }
    }

    public IEnumerator Movement(Vector3 destination)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, destination) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 1);
            yield return null;
        }
        isMoving = false;
    }
}

class Path : object
{
    public int g;         // Steps from A to this
    public int h;         // Steps from this to B
    public Path parent;   // Parent node in the path
    public int x;         // x coordinate
    public int y;         // y coordinate    public Path (int _g, int _h, Path _parent, int _x, int _y)

    public Path (int _g, int _h, Path _parent, int _x, int _y)
    {
        g = _g;
        h = _h;
        parent = _parent;
        x = _x;
        y = _y;
    }
    public int f // Total score for this
    {
        get
        {
            return g + h;
        }
    }
}
