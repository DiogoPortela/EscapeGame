using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    private bool canOpen = false;
    private bool isOpen = false;
    private int startRotating = 0;

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

        if (Input.GetKeyDown("e") && canOpen && !isOpen && startRotating == 0)
        {
            isOpen = true;
            startRotating = 1;
            //this.transform.Rotate(0,0,-90);
        }
        else if(Input.GetKeyDown("e") && canOpen && isOpen && startRotating == 0)
        {
            isOpen = false;
            startRotating = 2;
            //this.transform.Rotate(0, 0, 90);
        }

        if (startRotating == 1 && this.transform.eulerAngles.y < 90)
        {
            Debug.Log(this.transform.eulerAngles.y);
            this.transform.Rotate(0, 0, 5);
        }
        else if(startRotating == 1 && this.transform.eulerAngles.y >= 90)
        {
            startRotating = 0;
        }

        if(startRotating == 2 && this.transform.localEulerAngles.y > 0)
        {
            Debug.Log(this.transform.localEulerAngles.y);
            this.transform.Rotate(0, 0, -1);
        }

        else if(startRotating == 2 && this.transform.localEulerAngles.y <= 0)
        {
            startRotating = 0;
        }


    }
}
