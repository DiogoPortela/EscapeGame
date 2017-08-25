using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteInteract : MonoBehaviour, IInteractive {

    public MeshRenderer test;
    public bool wasCalled;

    private void Start()
    {
        test = GetComponent<MeshRenderer>();
        wasCalled = false;
    }

    public void Interact()
    {
        //test.material.color = new Color(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200));
        //wasCalled = !wasCalled;
    }
    public void InteractConstantly()
    {
        //test.material.color = new Color(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200));
        //wasCalled = !wasCalled;
    }

    public void InteractEnd()
    {
        //test.material.color = new Color(Random.Range(0, 200), Random.Range(0, 200), Random.Range(0, 200));
        //wasCalled = !wasCalled;
    }
}
