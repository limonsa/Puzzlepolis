using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TowerController : MonoBehaviour
{
    [SerializeField] private int towerNumber;
    [SerializeField] private HanoiGameManager gmHanoi;
    private Stack<GameObject> disks = new Stack<GameObject>();

    public static UnityAction<GameObject, int, string> ReceivingCollision;
    public static UnityAction NotifyingWin;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Disk")) {
            Debug.Log($"Invoking TowerConroller.ReceivingCollision (Disk{collision.gameObject.name}, Tower{towerNumber})");
            ReceivingCollision?.Invoke(collision.gameObject, towerNumber, "TowerController");
        }
    }

    private void Update()
    {
        if(towerNumber ==3 && disks.Count == 3)
        {
            NotifyingWin?.Invoke();
        }
    }

    public bool IsDiskInTower(GameObject disk)
    {
        bool rta = false;
        if (disks.Contains(disk))
        {
            rta = true;
        }
        return rta;
    }

    public bool IsNextLIFO(GameObject disk)
    {
        bool rta = false;
        if (disks.Count > 0)
        {
            if (disks.Peek().Equals(disk))
            {
                rta = true;
            }
        }
        return rta;
    }

    public float GetWeightNextLIFO()
    {
        float rta = -1;
        if (disks.Count > 0)
        {
            rta = disks.Peek().GetComponent<DiskController>().weight;
        }
        return rta;
    }

    public void SetPositionNextLIFO(Vector3 newPosition)
    {
        disks.Peek().GetComponent<DiskController>().transform.position = newPosition; ;
    }

    public GameObject PopFromStack()
    {
        GameObject rta = disks.Pop();
        if (disks.Count > 0)
        {
            disks.Peek().GetComponent<DiskController>().inPlay = true;
        }
        Debug.Log($"Removing Disk{rta.name} to tower {towerNumber} which now has {disks.Count} disks");
        return rta;
    }

    public void PushToStack(GameObject disk)
    {
        disk.GetComponent<DiskController>().tower = towerNumber;
        disk.GetComponent<DiskController>().inPlay = true;
        if (disks.Count > 0) {
            disks.Peek().GetComponent<DiskController>().inPlay = false;
        }
        disks.Push(disk);
        Debug.Log($"Adding Disk{disk.name} to tower {towerNumber}={disk.GetComponent<DiskController>().tower} which now has {disks.Count} disks");
    }

    public int GetTowerNumber()
    {
        return towerNumber;
    }

    public string GetDiskNames()
    {
        string rta = $"Tower{towerNumber} has {disks.Count} disks";
        return rta;
    }

    public int GetDisksCount()
    {
        return disks.Count;
    }
}
