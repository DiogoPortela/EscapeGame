using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int MovementSpeed = 1;
    public int RotateSpeed = 2;
    public int jumpingForce = 2;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        momentum = Vector3.zero;
        rigidBody = GetComponent<Rigidbody>();
        lookingTarget_empty = GameObject.Find("PickUpTarget");

        isCarring = false;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Carry();
        Movement();
    }

    private void Movement()
    {
        isFalling = !Physics.Raycast(this.transform.position + new Vector3(0, -0.7f, 0), Vector3.down, 0.3f);

        if (!isFalling)
        {
            if (Input.GetKey(KeyCode.Space))
                rigidBody.AddForce(new Vector3(0, jumpingForce, 0));
            momentum.Set(Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime);
        }

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

        if (Input.GetKey(KeyCode.Z) && lookingTarget.layer == 8)
        {
            if (!isCarring)
            {
                lookingTarget_RigidBody = lookingTarget.GetComponent<Rigidbody>();
                lookingTarget_RigidBody.useGravity = false;
                lookingTarget_RigidBody.velocity = Vector3.zero;
                isCarring = true;
            }

            lookingTarget.transform.position += (lookingTarget_empty.transform.position - lookingTarget.transform.position) * 5 * Time.deltaTime;

            //lookingTarget.transform.position = lookingTarget_empty.transform.position;
        }
        else if (isCarring)
        {
            lookingTarget_RigidBody.AddForce((lookingTarget_empty.transform.position - lookingTarget.transform.position) * 40);
            lookingTarget_RigidBody.useGravity = true;
            lookingTarget_RigidBody = null;
            lookingTarget = null;
            isCarring = false;
        }
    }
}
