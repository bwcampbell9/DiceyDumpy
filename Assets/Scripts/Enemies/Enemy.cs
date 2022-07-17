using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float range;

    private Player player;

    private GameObject target;
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position, out hit, range)) {
            Player pl = hit.collider.gameObject.GetComponent<Player>();
            if (pl != null)
            {
                target = hit.collider.gameObject;
            } else
            {
                target = null;
            }
        } else
        {
            target = null;
        }
        if(target)
        {
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
