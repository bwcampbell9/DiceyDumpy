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

    public int MAX_HEALTH = 100;
    public int health;
    public LayerMask bulletLayer;


    public GameObject topPoint;
    public GameObject leftPoint;
    public GameObject rightPoint;

    public GameObject[] gunObjs = new GameObject[3];
    public Gun[] guns = new Gun[3];
    public Weapon[] weapons = new Weapon[3];

    public Side equippedSide;
    public enum AttachmentPoints
    {
        rightPoint,
        topPoint,
        leftPoint,
    }

    public enum Side
    {
        right,
        top,
        left,
        legs,
        grapple,
        ass
    }

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    private float deadZone = .01f;

    private bool rotating = false;

    void Start()
    {
        health = MAX_HEALTH;
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

        equippedSide = (Side) GetSideForward();
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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (bulletLayer == (bulletLayer | (1 << collision.gameObject.layer)))
        {
            health -= collision.gameObject.GetComponent<Bullet>().explosionDamage;
            Destroy(collision.gameObject);
        }

    }

    void attachGun(Weapon weapon, AttachmentPoints point)
    {
        int index = (int)point;
        weapons[index] = weapon;

        gunObjs[index] = Instantiate(weapon.prefab, new Vector3(0, 0, 0), new Quaternion());

        if (point == AttachmentPoints.leftPoint)
        {
            gunObjs[index].transform.parent = leftPoint.transform;
        }
        else if (point == AttachmentPoints.topPoint)
        {
            gunObjs[index].transform.parent = topPoint.transform;
        }
        else
        {
            gunObjs[index].transform.parent = rightPoint.transform;
        }
        gunObjs[index].transform.localRotation = new Quaternion();
        gunObjs[index].transform.localPosition = new Vector3(0, 0, 0);

        guns[index] = gunObjs[index].GetComponent<Gun>();
    }
    IEnumerator FalseAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        rotating = false;
    }

    public int GetSideForward()
    {
        int side = GetIndexOfLowestValue(new[] {
            Vector3.Angle(objectToRotate.transform.right, transform.forward),
            Vector3.Angle(objectToRotate.transform.up, transform.forward),
            Vector3.Angle(-objectToRotate.transform.right, transform.forward),
            Vector3.Angle(-objectToRotate.transform.up, transform.forward),
            Vector3.Angle(objectToRotate.transform.forward, transform.forward),
            Vector3.Angle(-objectToRotate.transform.forward, transform.forward),
                });
        return side;
    }

    private int GetIndexOfLowestValue(float[] arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < value)
            {
                index = i;
                value = arr[i];
            }
        }
        return index;
    }
}
