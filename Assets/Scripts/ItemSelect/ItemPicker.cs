using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemPicker : MonoBehaviour
{
    public LayerMask dragable;
    public float lineSmoothness = 2;
    public float heightFactor = .1f;

    private bool dragging = false;
    private LineRenderer lineRenderer;
    private Vector3 endPoint;
    GameObject dragObject;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("down");
            if(!dragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, dragable))
                {
                    dragging = true;
                    dragObject = hit.collider.gameObject;
                    Debug.Log(dragObject);
                } else
                {
                    lineRenderer.positionCount = 0;
                }
            } else
            {
                //Debug.Log("dragging");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    endPoint = Vector3.Lerp(endPoint, hit.point, .3f);   
                    DrawLine(dragObject.transform.position, endPoint);
                }
            }
            
        } else
        {
            dragging = false;
            lineRenderer.positionCount = 0;
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        int pointCount = Mathf.CeilToInt(dist * lineSmoothness);
        lineRenderer.positionCount = pointCount;
        for(int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, SampleParabola(start, end, dist * heightFactor, (float)(i)/(pointCount-1)));
        }
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirection);
            Vector3 up = Vector3.Cross(right, levelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }
}
