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

    public static UnityAction<GameObject, int, string> ReceivingCollision;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        int numContacts = collision.GetContacts(contacts);

        if (inPlay && !gmHanoi.IsMovingBackState())
        {
            for (int i = 0; i < numContacts; i++)
            {
                if (Mathf.Abs(contacts[i].point.y - collisionChecker.position.y) < 0.015f) {
                    i = numContacts;
                    ReceivingCollision?.Invoke(collision.gameObject, tower, "DiskController");
                }
                
            }
        }
    }

    public void SetTower(int value)
    {
        tower = value;
    }
}
