using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEngine : MonoBehaviour
{


    Rigidbody arrowRB;
    [SerializeField]
    float arrowThrust = 20f;

    

    // Start is called before the first frame update
    void Start()
    {
        
        arrowRB = GetComponent<Rigidbody>();
        arrowRB.AddForce(-transform.forward * arrowThrust * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
		
    }
}
