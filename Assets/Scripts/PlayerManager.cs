using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int MovementSpeed = 1;
    public int RotateSpeed = 2;
    public float jumpingForce = 2;
    public float throwingForce = 0.7f;

    //  Movement
    private Vector3 momentum;
    private bool isFalling;
    private Rigidbody rigidBody;
    private float rotationXClampValue;
    // Carry
    private bool isCarring;
    private RaycastHit lookingInfo;
    private GameObject lookingTarget_empty;
    private GameObject lookingTarget;
    private Rigidbody lookingTarget_RigidBody;
    private bool justDropped;
    // Interact
    private IInteractive interactionScript;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        momentum = Vector3.zero;
        rigidBody = GetComponent<Rigidbody>();
        lookingTarget_empty = GameObject.Find("PickUpTarget");

        isCarring = false;
        justDropped = false;
    }

    private void FixedUpdate()
    {
        Carry();
        Interact();
        Movement();
    }

    private void Movement()
    {
        isFalling = !Physics.Raycast(this.transform.position + new Vector3(0, -0.7f, 0), Vector3.down, 0.4f);
        if (!isFalling)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rigidBody.AddRelativeForce(new Vector3(0, jumpingForce, 0) /*+ momentum*/, ForceMode.Impulse);
            }
        }

        momentum.Set(Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime);
        if (momentum != Vector3.zero)
        {
            this.transform.Translate(momentum);
        }

        if (Input.GetAxis("Mouse X") != 0)
        {
            this.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * RotateSpeed, 0));
        }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            rotationXClampValue += Input.GetAxis("Mouse Y");
            rotationXClampValue = Mathf.Clamp(rotationXClampValue, -20, 20);

            Camera.main.transform.localEulerAngles = new Vector3(-rotationXClampValue * RotateSpeed, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z);
            //Camera.main.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * RotateSpeed, 0, 0));
        }
    }
    private void Carry()
    {
        if (!isCarring)
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out lookingInfo, 2f))
                lookingTarget = lookingInfo.collider.gameObject;

        if (lookingTarget && Input.GetKey(KeyCode.Mouse0) && lookingTarget.layer == 8 && !justDropped)
        {
            if (!isCarring)
            {
                lookingTarget_RigidBody = lookingTarget.GetComponent<Rigidbody>();
                lookingTarget_RigidBody.useGravity = false;
                lookingTarget_RigidBody.velocity = Vector3.zero;
                isCarring = true;
            }

            else if (Input.GetKey(KeyCode.Mouse1))
            {
                lookingTarget_RigidBody.AddForce((lookingTarget_empty.transform.forward) * throwingForce, ForceMode.Impulse);
                lookingTarget_RigidBody.useGravity = true;
                lookingTarget_RigidBody = null;
                lookingTarget = null;
                isCarring = false;
                justDropped = true;
            }
            else
                lookingTarget.transform.position += (lookingTarget_empty.transform.position - lookingTarget.transform.position) * 5 * Time.deltaTime;

        }
        else if (justDropped && Input.GetKeyUp(KeyCode.Mouse0))
            justDropped = false;
        else if (isCarring)
        {
            lookingTarget_RigidBody.AddForce((lookingTarget_empty.transform.position - lookingTarget.transform.position) * throwingForce, ForceMode.Impulse);
            lookingTarget_RigidBody.useGravity = true;
            lookingTarget_RigidBody = null;
            isCarring = false;
        }
    }
    private void Interact()
    {
        if (lookingTarget)
        {
            if (Input.GetKeyDown(KeyCode.E) && (interactionScript = lookingTarget.GetComponent<IInteractive>()) != null)
                interactionScript.Interact();
            else if (Input.GetKeyUp(KeyCode.E) && (interactionScript = lookingTarget.GetComponent<IInteractive>()) != null)
                interactionScript.InteractEnd();
            else if (lookingTarget && Input.GetKey(KeyCode.E) && (interactionScript = lookingTarget.GetComponent<IInteractive>()) != null)
                interactionScript.InteractConstantly();
        }
    }
}
