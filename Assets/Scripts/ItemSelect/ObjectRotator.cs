using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectRotator : MonoBehaviour 
{
    public float torque = 1.0f;
    private float baseAngle = 0.0f;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {

    }

    void OnMouseDrag()
    {
        rb.AddTorque(Vector3.up * torque * -Input.GetAxis("Mouse X"));

        rb.AddTorque(Vector3.right * torque * Input.GetAxis("Mouse Y"));
    }
}