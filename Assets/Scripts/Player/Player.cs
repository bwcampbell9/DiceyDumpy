using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 1000.0f;
    public float clampAngle = 80.0f;
    public GameObject objectToRotate;
    public float tapDelay = .3f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    private float deadZone = .01f;

    private bool rotating = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        UpdateRotation();
    }

    void UpdateRotation()
    {
        if (!rotating)
        {
            float horiz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            if(!rotating)
            {
                if (horiz > deadZone)
                {
                    rotating = true;
                    objectToRotate.transform.RotateAround(objectToRotate.transform.position, transform.up, 90);
                    StartCoroutine(FalseAfterTime(tapDelay));
                }
                else if (horiz < -deadZone)
                {
                    rotating = true;
                    objectToRotate.transform.RotateAround(objectToRotate.transform.position, transform.up, -90);
                    StartCoroutine(FalseAfterTime(tapDelay));
                }
                else
                {
                    rotating = false;
                }

                if (vert > deadZone)
                {
                    rotating = true;
                    objectToRotate.transform.RotateAround(objectToRotate.transform.position, transform.right, 90);
                    StartCoroutine(FalseAfterTime(tapDelay));
                }
                else if (vert < -deadZone)
                {
                    rotating = true;
                    objectToRotate.transform.RotateAround(objectToRotate.transform.position, transform.right, -90);
                    StartCoroutine(FalseAfterTime(tapDelay));
                }
            }
        }

        //objectToRotate.localRotation = Quaternion.Euler(visualOffset);
    }

    IEnumerator FalseAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        rotating = false;
    }
}
