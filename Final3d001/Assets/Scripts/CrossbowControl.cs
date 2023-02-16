using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowControl : MonoBehaviour
{

    
    [SerializeField]
    Transform arrowSpawnPointPos;
    [SerializeField]
    GameObject arrowPrefab;

    private Animator myAnim;


    // Start is called before the first frame update
    void Start()
    {
        
        myAnim = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Fire1"))
		{
            Instantiate(arrowPrefab, arrowSpawnPointPos.position, Quaternion.Euler(0, transform.rotation.y, 0));
            
        }
		if (Input.GetButtonDown ("Fire1"))
		{
            myAnim.SetBool("FireCross", true);
        }
		if (Input.GetButtonUp("Fire1"))
        {
            myAnim.SetBool("FireCross", false);
        }

        
    }
}
