using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Addend : MonoBehaviour
{
    [HideInInspector] public List<AddendEdge> edges = new List<AddendEdge>();
    [SerializeField] private GameObject textPrefab;

    private GameObject addendGameObject;

    private void Awake()
    {
        addendGameObject = Instantiate(textPrefab);
        addendGameObject.GetComponent<TMP_Text>().SetText(gameObject.name);

        foreach(AddendEdge edge in FindObjectsOfType(typeof(AddendEdge)))
        {
            if(edge.addendFrom == this || edge.addendTo == this)
            {
                edges.Add(edge);
            }
        }
    }

    private void Update()
    {
        addendGameObject.transform.position = transform.position + Vector3.right * 0.055f + Vector3.forward * 0.05f;
    }
}
