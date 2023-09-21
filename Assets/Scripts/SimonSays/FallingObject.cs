using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingObject : MonoBehaviour
{
    public static UnityAction<GameObject, GameObject> ReportingCollision;

    private void OnCollisionEnter(Collision collision)
    {
        ReportingCollision?.Invoke(gameObject, collision.gameObject);
    }
}
