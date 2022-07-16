using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 1000.0f;
    public float clampAngle = 80.0f;
    public Transform objectToRotate;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    private float deadZone = .01f;

    private Vector3 rotOffset = new Vector3 (0,0,0);
    private Vector3 visualOffset = new Vector3 (0,0,0);
    private bool rotating = false;

    private Vector3 XRot = new Vector3(90, 0, 0), YRot = new Vector3(0, 90, 0), ZRot = new Vector3(0, 0, 90);

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
            if (horiz > deadZone)
            {
                // Clockwise Y
                rotating = true;
                LeanTween.value(gameObject, updateValueExampleCallback, rotOffset, rotOffset + YRot, .6f).setEase(LeanTweenType.easeOutElastic).setOnComplete(onRotationComplete);
                void updateValueExampleCallback(Vector3 val)
                {
                    visualOffset = val;
                    Debug.Log("tweened value:" + visualOffset + " percent complete:" + rotOffset);
                }
                void onRotationComplete()
                {
                    rotating = false;
                    rotOffset = visualOffset;
                    //Adjust Axis
                    Vector3 temp = XRot;
                    XRot = ZRot;
                    ZRot = temp;
                }
            }
            else if (horiz < -deadZone)
            {
                // CounterClockwise Y
                rotating = true;
                LeanTween.value(gameObject, updateValueExampleCallback, rotOffset, rotOffset - YRot, .6f).setEase(LeanTweenType.easeOutElastic).setOnComplete(onRotationComplete);
                void updateValueExampleCallback(Vector3 val)
                {
                    visualOffset = val;
                    Debug.Log("tweened value:" + visualOffset + " percent complete:" + rotOffset);
                }
                void onRotationComplete()
                {
                    rotating = false;
                    rotOffset = visualOffset;
                    //Adjust Axis
                    //Adjust Axis
                    Vector3 temp = XRot;
                    XRot = ZRot;
                    ZRot = temp;
                }
            }

            if (vert > deadZone)
            {
                // Clockwise X
                rotating = true;
                LeanTween.value(gameObject, updateValueExampleCallback, rotOffset, rotOffset + XRot, .6f).setEase(LeanTweenType.easeOutElastic).setOnComplete(onRotationComplete);
                void updateValueExampleCallback(Vector3 val)
                {
                    visualOffset = val;
                    Debug.Log("tweened value:" + visualOffset + " percent complete:" + rotOffset);
                }
                void onRotationComplete()
                {
                    rotating = false;
                    rotOffset = visualOffset;
                    //Adjust Axis

                    //Adjust Axis
                    Vector3 temp = YRot;
                    ZRot = -YRot;
                    YRot = temp;
                }
            }
            else if (vert < -deadZone)
            {
                    // CounterClockwise X
                    rotating = true;
                    LeanTween.value(gameObject, updateValueExampleCallback, rotOffset, rotOffset - XRot, .6f).setEase(LeanTweenType.easeOutElastic).setOnComplete(onRotationComplete);
                    void updateValueExampleCallback(Vector3 val)
                    {
                        visualOffset = val;
                        Debug.Log("tweened value:" + visualOffset + " percent complete:" + rotOffset);
                    }
                    void onRotationComplete()
                    {
                        rotating = false;
                        rotOffset = visualOffset;
                        //Adjust Axis
                        //Adjust Axis
                        Vector3 temp = -ZRot;
                        ZRot = YRot;
                        YRot = temp;
                    }
            }
        }

        objectToRotate.localRotation = Quaternion.Euler(visualOffset);
    }
}
