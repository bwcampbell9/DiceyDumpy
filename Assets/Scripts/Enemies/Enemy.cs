using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float range;
    public Gun gun;
    public LayerMask sightBlockers;

    public int MAX_HEALTH = 100;
    public int health;
    public LayerMask bulletLayer;

    private Player player;

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        RaycastHit closestValidHit = new RaycastHit();
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, range, sightBlockers);
        foreach (RaycastHit hit in hits)
        {
            if (!hit.transform.IsChildOf(transform) && (closestValidHit.collider == null || closestValidHit.distance > hit.distance))
            {
                closestValidHit = hit;
            }
        }
        if (closestValidHit.collider != null)
        {
            Player pl = closestValidHit.transform.gameObject.GetComponent<Player>();
            if (pl != null)
            {
                target = closestValidHit.transform.gameObject;
            }
            else
            {
                target = null;
            }
        }
        else
        {
            if(closestValidHit.collider != null)
            {
                Debug.Log(closestValidHit.collider.gameObject);
            }
            target = null;
        }
        if(target)
        {
            transform.LookAt(target.transform.position);
            gun.TryShoot(gun.transform.forward);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (bulletLayer == (bulletLayer | (1 << collision.gameObject.layer)))
        {
            health -= collision.gameObject.GetComponent<Bullet>().explosionDamage;
            Destroy(collision.gameObject);
        }

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
