using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public float force;
    public float duration;
    public Rigidbody rb;
    public Transform cubeTransform;
    public bool isFarting = false;
    private float timeFarting;
    public ParticleSystem fartCloud;
    public float chance = 0.01f;

    float ellapsed = 0;
    public float FREQUENCY = 1;

    float peak;
    float step;
    bool isHumping = false;
    bool peaked;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fart();
        }
    }

    private void FixedUpdate()
    {

        ellapsed += Time.fixedDeltaTime;

        if (ellapsed >= FREQUENCY)
        {
            ellapsed = 0;

            if (Random.Range(0f, 1f) < chance)
            {
                fart();
            }

            hump();
        }

        if (isFarting)
        {
            timeFarting += Time.fixedDeltaTime;

            int numParticles = Random.Range(1, 5);
            fartCloud.Emit(numParticles);

            if (timeFarting >= duration)
            {

                isFarting = false;
                return;
            }

            rb.AddForce(force * cubeTransform.forward);
        }
    }

    void hump()
    {
        if (!isHumping)
        {
            peaked = false;
            peak = Random.Range(0.6f, 0.8f);
            step = Random.Range(0.01f, 0.05f);
            isHumping = true;
        }

        if (!peaked)
        {
            if (chance < peak)
            {
                chance += step;
            }
            else
            {
                peaked = true;
            }
        }
        else
        {
            if (chance > 0.1)
            {
                chance -= step;
            }
            else
            {
                isHumping = false;
            }
        }


    }

    void fart()
    {
        fartCloud.Emit(50);
        timeFarting = 0;
        isFarting = true;
    }

}
