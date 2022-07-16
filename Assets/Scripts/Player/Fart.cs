using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public float force;
    public float duration;
    public Rigidbody rb;
    public bool isFarting = false;
    private float timeFarting;
    public ParticleSystem fartCloud;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        Debug.DrawLine(transform.position, transform.position + -1 * transform.forward, Color.blue);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("We fartin");
            fart();
        }
    }

    private void FixedUpdate()
    {
        if (isFarting)
        {
            timeFarting += Time.deltaTime;

            int numParticles = Random.Range(1, 5);
            fartCloud.Emit(numParticles);

            if (timeFarting >= duration)
            {

                isFarting = false;
                return;
            }

            rb.AddForce(force * transform.forward);
        }
    }

    void fart()
    {
        fartCloud.Emit(50);
        timeFarting = 0;
        isFarting = true;
    }

}
