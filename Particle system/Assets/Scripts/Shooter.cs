using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject arrow;
    public float shootForce = 0f;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = (GameObject)Instantiate(arrow, transform.position, transform.rotation);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * shootForce);
        }
		
	}
}
