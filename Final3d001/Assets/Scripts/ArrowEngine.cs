using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEngine : MonoBehaviour
{

    float penetrationDepth = 0.5f;

    [SerializeField]
    private float damage = 34f;




    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
		


    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit!");

        //if (!collision.gameObject.CompareTag("ArrowHitAble")) return;

        GameObject arrow = gameObject;
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        if (arrowRigidbody)
        {
            arrowRigidbody.isKinematic = true;
        }

        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion contactRotation = Quaternion.LookRotation(collision.contacts[0].normal, transform.up);

        arrow.transform.parent = collision.transform;
        arrow.transform.position = contactPoint;

        // Align arrow with contact normal
        arrow.transform.rotation = contactRotation * Quaternion.Euler(0, 0, -90);

        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null && target.tag == "ArrowHitAble")
        {
            target.TakeDamage(damage);
        }
    }


}
