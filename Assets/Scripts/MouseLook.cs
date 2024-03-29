using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public PlayerScript ps;
    public float sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ps.isMoving || ps.isRotating || Input.GetMouseButtonUp(1))
        {
            transform.localRotation = Quaternion.identity;
        }
        else if (Input.GetMouseButton(1))
        {
            transform.localRotation = Quaternion.AngleAxis(transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity, Vector3.up);
        }
    }
}
