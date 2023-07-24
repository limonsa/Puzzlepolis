using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Setup")]
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform head;
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
        //Up and down rotation with mouse:
        xRotation = head.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;
        float currentXrotation = head.transform.eulerAngles.x;
        //Limit the liberty for looking up and down:
        //xRotation = Mathf.Clamp(xRotation, -rotationLimit, rotationLimit);
        Debug.Log($"xRotation = {xRotation}");

        if(xRotation > rotationLimit && xRotation < 360 - rotationLimit)
        {
            Debug.Log($"Entro a uno con {head.transform.eulerAngles.x} / {xRotation}");
            if (xRotation > 180)
            {
                Debug.Log($"Entro a DOS con {head.transform.eulerAngles.x} / {xRotation}");
                xRotation = 360 - rotationLimit;
                Debug.Log($"xRotation quedo en {xRotation}");
            }
            else
            {
                Debug.Log($"Entro a  t r e s  con {head.transform.eulerAngles.x} / {xRotation}");
                xRotation = rotationLimit;
                Debug.Log($"xRotation quedo en {xRotation}");
            }

        }
        Debug.Log($"xRotation quedo en {xRotation}");
        head.transform.eulerAngles = new Vector3(xRotation, 0, 0);
        Debug.Log($"HEAD ROTO A {head.transform.eulerAngles.x} / {xRotation}");
    }


    public void MovePlayer()
    {
        Vector3 direction = new Vector3(0, 0, Input.GetAxis("Vertical"));
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }

    private void ControlSpeed()
    {
        Vector3 currentSpeed = new Vector3(player.velocity.x, 0f, player.velocity.z);
        //Check the Rigidbody speed (player speed) is moving faster than the moveSpeed
        //caused for aceleration
        if (currentSpeed.magnitude > movementSpeed)
        {
            Vector3 speedLimit = currentSpeed.normalized * movementSpeed;
            player.velocity = new Vector3(speedLimit.x, player.velocity.y, speedLimit.z);
        }
    }
}