using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 10f;

    private Rigidbody myRigidbody;
    private Transform tempShowTransform;

    public static UnityAction HoldingObject;
    public static UnityAction DroppingObject;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform pickableObjectTransform)
    {
        tempShowTransform = pickableObjectTransform;
        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
        HoldingObject?.Invoke();
    }

    public void Drop()
    {
        tempShowTransform = null;
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;
        DroppingObject?.Invoke();
    }

    private void FixedUpdate()
    {
        if (tempShowTransform != null && (Vector3.Distance(transform.position, tempShowTransform.position) > 0.001))
        {
            Vector3 newPos = Vector3.Lerp(transform.position, tempShowTransform.position, Time.deltaTime * lerpSpeed);
            myRigidbody.MovePosition(newPos);
        }
    }
}