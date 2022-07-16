using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public Rigidbody rb;

    private void FixedUpdate()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        foreach (Attractor attractor in attractors)
        {
            if(attractor != this)
            {
                Attract(attractor);
            }
        }
    }
    void Attract(Attractor other)
    {
        Rigidbody otherRB = other.rb;

        Vector3 direction = rb.position - otherRB.position;
        float distance = direction.magnitude;

        float forceMagnitude = (rb.mass * otherRB.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        otherRB.AddForce(force);
    }
}
