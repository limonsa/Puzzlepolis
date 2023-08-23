using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskContainer : MonoBehaviour
{
    [SerializeField] private GameObject diskCollisionPoint;

    public DiskController GetDiskController()
    {
        return diskCollisionPoint.GetComponent<DiskController>();
    }
}
