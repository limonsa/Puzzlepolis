using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingObject : Playable
{
    private string nameID;
    public static UnityAction<GameObject, GameObject> ReportingCollision;

    public override void ActivateRole()
    {
        throw new System.NotImplementedException();
    }

    public override string GetNameID()
    {
        return nameID;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        nameID = collision.gameObject.tag;
        ReportingCollision?.Invoke(gameObject, collision.gameObject);
    }
}
