using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckerController : MonoBehaviour
{
    [SerializeField] private DiskController disk;

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{disk.gameObject.name} detected collision with {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Disk"))
        {
            disk.DetectingCollisionWith(collision.gameObject);
        }
    }*/
}
