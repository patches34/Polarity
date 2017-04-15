using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class MetalObject : MonoBehaviour
{
    Rigidbody rb;
    public Polarity polarity;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Magnet"))
        {
            rb.AddForce(other.GetComponent<MagnetConstantForce>().GetForce() * (int)polarity, ForceMode.Force);
        }
    }
}
