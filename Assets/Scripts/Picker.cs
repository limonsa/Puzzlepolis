using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    [Header("Picker configuration")]
    [SerializeField] private Transform pickerCameraTransform;
    [SerializeField] private LayerMask pickerLayerMask;
    [SerializeField] private float pickupDistance = 1f;

    [Header("Picked object transition view")]
    [SerializeField] private Transform grabObjectPos;

    private Pickable objectPickable;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (objectPickable == null)
            {    //No object being picked => try to pick object
                if (Physics.Raycast(transform.position, transform.forward,
                                out RaycastHit raycastHit, pickupDistance, pickerLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectPickable))
                    {
                        objectPickable.Grab(grabObjectPos);
                    }
                }
            }
            else
            {  //Object currently being picked => drop the object
                objectPickable.Drop();
                objectPickable = null;
            }
        }
    }
}
