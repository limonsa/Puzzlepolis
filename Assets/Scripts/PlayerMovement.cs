using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Setup")]
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform head;
    [SerializeField] private Transform cameraPlayer;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Looking up & down Setup")]
    [SerializeField] private float sensitivityY = 400f;    //Mouse sensitivity across Y movement

    private float xRotation;
    private float yRotation;
    private float rotationLimit = 40f;

    public void FixedUpdate()
    {
        MovePlayer();
    }

    public void Update()
    {
        RotatePlayer();
        ControlSpeed();
    }

    public void RotatePlayer()
    {
        //Left and right rotation with keyboard:
        yRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        player.transform.Rotate(0, yRotation, 0);

        //Up and down rotation with mouse:
        xRotation = head.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;

        //Limit the liberty for looking up and down:
        //Clamp does not work becuse transform.Rotation works with absolute [or positive] angle values
        //xRotation = Mathf.Clamp(xRotation, -rotationLimit, rotationLimit);

        if (xRotation > rotationLimit && xRotation < 360 - rotationLimit)
        {
            if (xRotation > 180)
            {
                xRotation = 360 - rotationLimit;
            }
            else
            {
                xRotation = rotationLimit;
            }
        }
        head.transform.eulerAngles = new Vector3(xRotation, player.transform.localEulerAngles.y, 0);
    }


    public void MovePlayer()
    {
        Vector3 direction = new Vector3(0, 0, Input.GetAxis("Vertical"));
        transform.Translate(direction * movementSpeed * Time.deltaTime);

    }

    private void ControlSpeed()
    {
        Vector3 currentSpeed = new Vector3(0f, 0f, player.velocity.z);
        //Check the Rigidbody speed (player speed) is moving faster than the moveSpeed
        //caused for aceleration
        if (currentSpeed.magnitude > movementSpeed)
        {
            Vector3 speedLimit = currentSpeed.normalized * movementSpeed;
            player.velocity = new Vector3(speedLimit.x, player.velocity.y, speedLimit.z);
        }
    }
}