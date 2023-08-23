using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HanoiGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] disks;
    [SerializeField] private TowerController[] towers;
    public static Action<GameObject, int> MovingPlateInTower;

    private void Start()
    {
        TowerController.MovingPlateInGame += MoveDisk;
        DiskController.PlacingDisk += SetupDisk;
    }

    private void MoveDisk(GameObject diskMoving, int fromTower, int toTower)
    {
        if (towers[fromTower].TakeAwayDisk(diskMoving, fromTower))
        {
            towers[toTower].AddDiskToTower(diskMoving, toTower);
        }
        else {
            for(int i=0; i<disks.Length; i++)
            {
                if (diskMoving.GetComponent<DiskController>().name ==
                    disks[i].GetComponent<DiskController>().name)
                {
                    disks[i].GetComponent<DiskController>().MoveBack();
                    i = disks.Length;
                }
            }
        }
    }

    private void SetupDisk(GameObject newDisk, int toTower)
    {
        towers[0].SetupDisk(newDisk);
    }
}
