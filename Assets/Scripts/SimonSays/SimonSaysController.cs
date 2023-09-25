using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using System;
using TMPro;

public class SimonSaysController : MonoBehaviour
{
    [SerializeField] private GameObject stool; //Tag is Disk
    [SerializeField] private GameObject suitcase;   //Tag is Disk
    [SerializeField] private GameObject bluePlatform;
    [SerializeField] private GameObject yellowPlatform;
    [SerializeField] private GameObject player;

    [Header ("End game setup")]
    [SerializeField] private GameObject winCutscene;
    [SerializeField] private GameObject loseCutscene;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject confetti;
    [SerializeField] private GameObject explosion;
    [SerializeField] private TMP_Text endText;

    private PlayableDirector director;
    private string[] steps = new string[2];
    private Queue<string> instructions = new Queue<string>();
    private GameObject[] reported = new GameObject[2];
    private int currentStep = 0;
    private StepState progress = StepState.None;
    private bool reportedBlue = false;
    private bool reportedYellow = false;

    private static SimonSaysController instance = null;
    private static readonly object padlock = new object();

    SimonSaysController()
    {
    }

    public static SimonSaysController Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new SimonSaysController();
                }
                return instance;
            }
        }
    }

    private void Start()
    {
        FallingObject.ReportingCollision += HandlingCollision;
        confetti.SetActive(false);
        director = winCutscene.GetComponent<PlayableDirector>();
        SetInstructions();
    }

    public void HandlingCollision(GameObject reporter, GameObject other)
    {
        int i = -1;
        //foreach (string s in steps)
        //{
        //    Debug.Log($"steps has: {s}, Reporter is {reporter.name}, Other is {other.name} and currentStep={currentStep}");

        //}

        if(reporter.CompareTag("Base") && other.CompareTag("Disk"))
        {
            i = Int32.Parse(reporter.name);
            if (reported[i] == null)
            {
                reported[i] = other;                //Saves the falling object on the i-platform
                steps[currentStep] = reporter.name; //Saves the order of falling platform
                currentStep++;
            }
        }

        if (currentStep == 2)
        {
            CheckIfWon();
        }
    }

    public void CheckIfWon()
    {
        bool won = false;
        player.SetActive(false);

        //Debug.Log($"CheckIfWon says: steps[0]={steps[0]}(must be blue) and steps[0]={steps[0]}(must be yellow) ALSO reported[blue].={reported[0].name}(must be suit) and reported[yellow].={reported[1].name}(must be stool)");
        if (steps[0].Equals(bluePlatform.name) && steps[1].Equals(yellowPlatform.name)) //platform order check
        {
            if (reported[0].Equals(suitcase) && reported[1].Equals(stool))  // Correct object on platform check
            {
                won = true;
            }
        }
        if (won){
            confetti.SetActive(true);
            endText.text = "You won!";
            winCutscene.SetActive(true);
        }
        else
        {
            explosion.SetActive(true);
            endText.text = "You lost";
            loseCutscene.SetActive(true);
        }
        director.Play();
    }

    private void SetInstructions()
    {
        instructions.Enqueue(suitcase.name);
        instructions.Enqueue(stool.name);
    }
}

public enum StepState
{
    None,
    First,
    Second
}
