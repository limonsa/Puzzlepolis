using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class DiskController : MonoBehaviour
{
    [SerializeField] private GameObject disk;
    [SerializeField] private HanoiGameManager gmHanoi;
    [SerializeField] private Transform collisionChecker;

    public int weight;
    public int tower = 0;
    public bool inPlay = false;

    public static UnityAction<GameObject, int> ReceivingCollision;

    /*public static Action<GameObject, int, int> MoveDisk;
    public Transform lastTransform;
    
    */

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        int numContacts = collision.GetContacts(contacts);

        Debug.Log($"Disk{gameObject.name} (that is inPlay? {inPlay}) detected Collision with {collision.gameObject.name}");
        if (inPlay && !gmHanoi.isMovingBackState())
        {
            for (int i = 0; i < numContacts; i++)
            {
                Debug.Log($"Y-collision distance{Mathf.Abs(contacts[i].point.y - collisionChecker.position.y)}");
                if (Mathf.Abs(contacts[i].point.y - collisionChecker.position.y) < 0.015f) {
                    i = numContacts;
                    Debug.Log($"INVOKING: ordering to move Disk{collision.gameObject.name} to tower{tower}");
                    ReceivingCollision?.Invoke(collision.gameObject, tower);
                }
                
            }
        }
    }

    public void SetTower(int value)
    {
        tower = value;
    }
}
