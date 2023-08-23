using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class DiskController : MonoBehaviour
{
    [SerializeField] private GameObject disk;
    [SerializeField] private Transform collisionChecker;

    public int weight;
    public int tower;
    public static Action<GameObject, int> PlacingDisk;
    public Transform lastTransform;

    private void Start()
    {
        weight = int.Parse(disk.name);
        tower = 0;
        Pickable.GettingReadyToMove += SaveLastLocation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        int numContacts = collision.GetContacts(contacts);
        Debug.Log($"{gameObject.name} RECIBIO COLISION DE {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Disk")) {
            for(int i = 0; i < numContacts; i++)
            {
                Debug.Log("Detccion con un disco detectada");
                if (Vector3.Distance(contacts[i].point, collisionChecker.position) < 0.5f)
                {
                    Debug.Log($"{gameObject.name} COLISIONO CON {collision.gameObject.name} & weight = {collision.gameObject.GetComponent<DiskController>().weight}");
                    if (collision.gameObject.GetComponent<DiskController>().weight < weight)
                    {
                        Debug.Log($"va a a adicionar {collision.gameObject.name}");
                        PlacingDisk?.Invoke(collision.gameObject, tower);
                    }
                    else
                    {
                        Debug.Log($"ENTRO AL else porque el peso de {collision.gameObject.name} es mayor o igual a {weight}");
                        collision.gameObject.transform.position = collision.gameObject.GetComponent<DiskController>().lastTransform.position;
                    }
                }
            }            
        }
    }

    public void SaveLastLocation()
    {
        Debug.Log($"Saving {gameObject.name} current position in {tower}");
        lastTransform.position = disk.transform.position;
    }

    public void MoveBack()
    {
        disk.transform.position = lastTransform.position;
    }
}
