using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcTarget : Playable
{
    [SerializeField] private int[] addends = { 2, 7, 11, 15 };
    [SerializeField] private int target;

    // Start is called before the first frame update
    void Start()
    {
        int[] rtaAddends = findAddendsSubidex();
        //Debug.Log($"{rtaAddends[0]}, {rtaAddends[1]}");
    }

    // Update is called once per frame
    int[] findAddendsSubidex()
    {
        int[] rta = new int[2];

        for(int i = 0; i < addends.Length; i++)
        {
            for(int j = i; j < addends.Length; j++)
            {
                if (addends[i] + addends[j] == target)
                {
                    rta[0] = i;
                    rta[1] = j;
                    i = addends.Length;
                    j = addends.Length;
                }
            }
        }
        return rta;
    }

    public override string GetNameID()
    {
        return target.ToString();
    }

    public override void ActivateRole()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        throw new System.NotImplementedException();
    }
}
