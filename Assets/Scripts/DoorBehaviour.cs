using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    private bool canOpen = false;
    private bool isOpen = false;

    void OnTriggerEnter(Collider other)
    {
        canOpen = true;
        Debug.Log("Enters");
    }

    void OnTriggerExit(Collider other)
    {
        canOpen = false;
        Debug.Log("Exits");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("e") && canOpen && !isOpen)
        {
            isOpen = true;
            this.transform.Rotate(0,0,-90);
        }
        else if(Input.GetKeyDown("e") && canOpen && isOpen)
        {
            isOpen = false;
            this.transform.Rotate(0, 0, 90);
        }

    }
}
