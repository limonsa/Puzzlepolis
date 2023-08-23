using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TowerController : MonoBehaviour
{
    [SerializeField] private int towerNumber;
    private Stack<GameObject> disks = new Stack<GameObject>();
    //private Dictionary<GameObject, int> towers = new Dictionary<GameObject, int>();
    
    public static Action<GameObject, int, int> MovingPlateInGame;


    private void Start()
    {
        //HanoiGameManager.MovingPlateInTower += TakeAwayDisk;
        //DiskController.PlacingDisk += AddDiskToTower;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowDisksStacked();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Disk"))
        {
            if (!disks.Contains(collision.gameObject)) { 
                //collision.gameObject.GetComponent<DiskContainer>().GetDiskController().tower = towerNumber;
                collision.gameObject.GetComponent<DiskController>().tower = towerNumber;
                disks.Push(collision.gameObject);
                ShowDisksStacked();
            }
        }
    }

    public void AddDiskToTower(GameObject newDisk, int fromTower)
    {
        bool moveGranted = false;
        if(newDisk.GetComponent<DiskController>().tower == towerNumber) { 
            Debug.Log($"About to add disk {newDisk.gameObject.name} to {towerNumber}");
            if (!disks.Contains(newDisk))
            {
                try { 
                    //Let know the fromTower who has the disk that it is moving out -> must erase it
                    MovingPlateInGame?.Invoke(newDisk, fromTower, towerNumber);
                    moveGranted = true;
                }
                catch
                {
                    //failed attempt
                    moveGranted = false;
                }

                if (moveGranted) { 
                    newDisk.GetComponent<DiskController>().tower = towerNumber;
                    if (disks.Count == 0){
                        disks.Push(newDisk);
                    }
                    else if (disks.Peek().GetComponent<DiskController>().weight > newDisk.GetComponent<DiskController>().weight)
                    {
                        disks.Push(newDisk);
                    }
                }
                ShowDisksStacked();
            }
        }
    }

    public void SetupDisk(GameObject newDisk)
    {
        if (!disks.Contains(newDisk)) { 
            newDisk.GetComponent<DiskController>().tower = towerNumber;
            disks.Push(newDisk);
        }
    }

    public void ShowDisksStacked()
    {
        Debug.Log($"Tower #{towerNumber} has {disks.Count} disks");
    }

    public bool TakeAwayDisk(GameObject newDisk, int fromTower)
    {
        Debug.Log($"About to take away disk {newDisk.gameObject.name} from {fromTower}");
        if (fromTower == towerNumber &&
            newDisk.GetComponent<DiskController>().name == disks.Peek().GetComponent<DiskController>().name)
        {
            //Debug.Log($"About to take away disk {newDisk.gameObject.name} from {towerNumber}");
            disks.Pop();
            return true;
        }
        return false;
    }
}
