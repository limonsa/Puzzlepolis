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
                        //For HanoiGameTower to listen an decide
                        GettingReadyToMove?.Invoke(objectPickable.gameObject);
                        objectPickable.Grab(grabObjectPos);
                    }
                }
            }
            else
            {  //Object currently being picked => drop the object
                objectPickable.Drop();
                objectPickable = null;
                CheckWin?.Invoke();
            }
        }
    }
}
