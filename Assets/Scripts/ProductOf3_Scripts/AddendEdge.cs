using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class AddendEdge : MonoBehaviour
{
    [SerializeField] public Addend addendFrom;
    [SerializeField] public Addend addendTo;
    public float cost;
    [SerializeField] private GameObject textPrefab;

    private GameObject edgeLabel;
    private LineRenderer line;

    private void Start()
    {
        edgeLabel = Instantiate(textPrefab);
        edgeLabel.GetComponent<TMP_Text>().color = Color.black;
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    private void Update()
    {
        cost = Vector3.Distance(addendFrom.transform.position, addendTo.transform.position);
        DrawLine();
        edgeLabel.GetComponent<TMP_Text>().SetText("" + Mathf.RoundToInt(cost));
    }

    public void DrawLine()
    {
        line.SetPosition(0, addendFrom.transform.position + (Vector3.down * 0.02f));
        line.SetPosition(1, addendTo.transform.position + (Vector3.down * 0.02f));
        edgeLabel.transform.position = ((addendFrom.transform.position + addendTo.transform.position) / 2) + (Vector3.up * 0.02f);
    }
}
