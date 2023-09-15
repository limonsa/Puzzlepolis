using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Picker : MonoBehaviour
{
    [Header("Picker configuration")]
    [SerializeField] private Transform pickerCameraTransform;
    [SerializeField] private LayerMask pickerLayerMask;
    [SerializeField] private float pickupDistance = 2f;

    [Header("Picked object transition view")]
    [SerializeField] private Transform grabObjectPos;
    public HanoiGameManager hgmTEMP;

    public static UnityAction CheckWin;
    public static UnityAction<GameObject> GettingReadyToMove;

    private Pickable objectPickable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (objectPickable == null)
            {    //No object being picked => try to pick object
                if (Physics.Raycast(pickerCameraTransform.transform.position, pickerCameraTransform.transform.forward,
                                out RaycastHit raycastHit, pickupDistance))
                {
                    
                    if (raycastHit.transform.TryGetComponent(out objectPickable))
                    {
                        /*if (objectPickable.CompareTag("Disk"))
                        {
                            Debug.Log($"PICKER SAYS >>>>> POSITION of disk{objectPickable.name} in tower{objectPickable.GetComponent<DiskController>().tower} is {objectPickable.GetComponent<DiskController>().transform.position} AND LASTPOSITION in HanoiGameManager={hgmTEMP.logLastMove}");
                            GettingReadyToMove?.Invoke(objectPickable.gameObject);
                            //objectPickable.GetComponent<DiskController>().lastTransform = objectPickable.GetComponent<DiskController>().transform;
                        }*/
                        //For HanoiGameTower to listen an decide
                        GettingReadyToMove?.Invoke(objectPickable.gameObject);
                        objectPickable.Grab(grabObjectPos);
                        
                        //Debug.Log($"PICKER SAYS >>>>> lastPos now is = {objectPickable.GetComponent<DiskController>().lastTransform.position}");
                    }
                }
            }
            else
            {  //Object currently being picked => drop the object
                objectPickable.Drop();
                //TODO: delete, justo for Hanoi debuggin
                //Debug.Log($"PICKER SAYS AFTER DROPPING>>>>> position = {objectPickable.GetComponent<DiskController>().transform.position}  AND LASTPOSITION in HanoiGameManager={hgmTEMP.logLastMove}");
                objectPickable = null;
                CheckWin?.Invoke();
            }
        }
    }
}
