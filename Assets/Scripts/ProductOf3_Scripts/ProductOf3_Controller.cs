using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class ProductOf3_Controller : MonoBehaviour
{
    public List<Addend> addends = new List<Addend>();
    public List<Addend> winnerPath = new List<Addend>();
    [SerializeField] private GameObject portal;
    [SerializeField] private Transform positionPortalView;
    [SerializeField] private GameObject cameraGuide;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject player;
    [SerializeField] private Addend startAddend;
    [SerializeField] private Addend targetAddend;
    [SerializeField] private GameObject endCutscene;

    public LineRenderer path;

    private Dictionary<Addend, float> distances = new Dictionary<Addend, float>();
    private Dictionary<Addend, Addend> previousAddends = new Dictionary<Addend, Addend>();
    private List<Addend> shortestPath;

    PlayableDirector director;

    private static ProductOf3_Controller instance = null;
    private static readonly object padlock = new object();

    ProductOf3_Controller()
    {
    }

    public static ProductOf3_Controller Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ProductOf3_Controller();
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        portal.SetActive(false);
        foreach (Addend a in FindObjectsOfType(typeof(Addend)))
        {
            addends.Add(a);
        }
        Picker.CheckWin += CheckIfWon;
    }

    void Start()
    {
        director = endCutscene.GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        //if (startAddend == null || targetAddend == null) {
        //    Debug.Log("Entered Update");
        //}else {
        if (startAddend != null && targetAddend != null)
        {
            CalculateCheapestPath();
            DrawCheapestPath();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenPortal();
            ShowWinCutscene();
        }
    }

    public void CheckIfWon()
    {
        bool playerWon = true;
        if (winnerPath.Count != shortestPath.Count)
        {
            playerWon = false;
        }
        else { 
            for (int i = 0; i < winnerPath.Count; i++)
            {
                if (winnerPath[i].name != shortestPath[i].name)
                {
                    playerWon = false;
                    i = winnerPath.Count;
                }
            }
        }

        if (playerWon)
        {
            OpenPortal();
            ShowWinCutscene();
        }

    }

    public void CalculateCheapestPath()
    {
        //This method will use Dijkstra's algoritm to find the shortest path in a graph
        //The seed case in Dijkstra is set the the distance to the first node to itself to 0
        //AND all the other distances in the graph to infinity
        foreach (Addend a in addends)
        {
            distances[a] = float.MaxValue;
            previousAddends[a] = null;

        }
        distances[startAddend] = 0;

        //At the beginning all the graph nodes haven't been visited
        List<Addend> unvisitedAddends = new List<Addend>(addends);

        //Loop over the unvisited nodes
        while (unvisitedAddends.Count > 0)
        {
            Addend currentAddend = null;
            foreach(Addend unvisited in unvisitedAddends)
            {
                //If is the first node (start point of the search)
                //OR if the distance is shorted
                if(currentAddend == null || distances[unvisited] < distances[currentAddend])
                {
                    currentAddend = unvisited; 
                }
            }

            unvisitedAddends.Remove(currentAddend);
            
            //currentAddend ends up with the node before last of the shortest path from the origin to the i-unvisited node
            //Update distances to neighoring nodes for the node that
            foreach(AddendEdge edge in currentAddend.edges)
            {
                float distance = distances[currentAddend] + edge.cost;
                Addend neighbour = currentAddend == edge.addendFrom ? edge.addendTo : edge.addendFrom;
                if(distance < distances[neighbour])
                {
                    distances[neighbour] = distance;
                    previousAddends[neighbour] = currentAddend;
                }
            }
        }
    }

    public void DrawCheapestPath()
    {

        shortestPath = new List<Addend>();
        Addend pathAddend = targetAddend;
        while(pathAddend != null)
        {
            shortestPath.Insert(0, pathAddend);
            pathAddend = previousAddends[pathAddend];
        }
        path.positionCount = shortestPath.Count;
        for(int i = 0; i < shortestPath.Count; i++)
        {
            path.SetPosition(i, shortestPath[i].transform.position);
        }
    }

    public void OpenPortal()
    {
        portal.SetActive(true);

    }

    public void ShowWinCutscene() {
        cameraGuide.transform.position = positionPortalView.position;
        cameraGuide.transform.rotation = positionPortalView.rotation;
        //guide.transform.rotation = positionPortalView.rotation;
        director.Play();
    }
}
