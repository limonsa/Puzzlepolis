using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Playable: MonoBehaviour
{
    public abstract string GetNameID();
    public abstract void ActivateRole();
    public abstract void OnCollisionEnter(Collision collision);
}
