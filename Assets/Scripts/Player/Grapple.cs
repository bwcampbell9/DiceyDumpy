using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public Rigidbody playerBody;
    public float grappleSpeed;
    private float maxDistance = 100000f;
    private bool grappling = false;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("start Grapple");
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        Debug.DrawLine(camera.transform.position, camera.forward, Color.red, 0.0f, false);
    }

    private void FixedUpdate()
    {

        if (grappling)
        {
            Vector3 direction = gunTip.position - grapplePoint;
            float distance = direction.magnitude;

            float forceMagnitude = grappleSpeed;
            Vector3 force = direction.normalized * forceMagnitude;

            playerBody.AddForce(force);
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, whatIsGrappleable))
        {
            Debug.Log(hit);

            grapplePoint = hit.point;

            //float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            grappling = true;
        } else
        {
            Debug.Log("No target");
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        lr.positionCount = 0;
        grappling = false;
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!grappling) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
