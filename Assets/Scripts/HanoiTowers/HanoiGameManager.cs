using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.Playables;

public class HanoiGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] disksSetup;
    [SerializeField] private TowerController[] towers;

    [Header("Setting End of puzzle")]
    [SerializeField] private GameObject portal;
    [SerializeField] private Transform positionPortalView;
    [SerializeField] private GameObject cameraGuide;
    [SerializeField] private GameObject winCutscene;
    [SerializeField] private GameObject loseCutscene;
    [SerializeField] private GameObject timer;

    private PlayableDirector director;

    private bool movingBackState = false;
    public Vector3 logLastMove;
    private static HanoiGameManager instance = null;
    private static readonly object padlock = new object();

    HanoiGameManager()
    {
    }

    public static HanoiGameManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new HanoiGameManager();
                }
                return instance;
            }
        }
    }


    private void Start()
    {
        TowerController.ReceivingCollision += MakingMove;
        DiskController.ReceivingCollision += MakingMove;
        Picker.GettingReadyToMove += SavingLogLastMove;
        Picker.CheckWin += CheckingIfWon;
        TowerController.NotifyingWin += ShowWinCutscene;
        CountDown.TimerEnding += CheckingIfWonTimer;

        //Positioning the disks on the interface and the tower0's STACK of disks
        disksSetup[0].transform.position = new Vector3(-4.9f, 1.09f, 1f);
        disksSetup[1].transform.position = new Vector3(-4.9f, 1.01f, 1f);
        disksSetup[2].transform.position = new Vector3(-4.9f, 0.93f, 1f);
        towers[0].PushToStack(disksSetup[2]);
        towers[0].PushToStack(disksSetup[1]);
        logLastMove = disksSetup[0].transform.position;
        towers[0].PushToStack(disksSetup[0]);

        director = winCutscene.GetComponent<PlayableDirector>();
    }

    private void OnDestroy()
    {
        TowerController.ReceivingCollision -= MakingMove;
        DiskController.ReceivingCollision -= MakingMove;
        Picker.GettingReadyToMove -= SavingLogLastMove;
        Picker.CheckWin -= CheckingIfWon;
        TowerController.NotifyingWin -= ShowWinCutscene;
        CountDown.TimerEnding -= CheckingIfWonTimer;
    }
    private void Awake()
    {
        portal.SetActive(false);

    }

    private void MakingMove(GameObject disk, int toTower, string source)
    {
        int fromTower = LocateDisk(disk);
        float weightTo = 0, weightFrom = 0;
        if (!movingBackState || source.Equals("TowerController")) { 
            if (fromTower >= 0)
            {
                if (towers[fromTower].IsNextLIFO(disk) && !towers[toTower].IsDiskInTower(disk))
                {
                    weightFrom = towers[fromTower].GetWeightNextLIFO();
                    weightTo = towers[toTower].GetWeightNextLIFO();
                    if(weightTo > 0) {
                        if (weightTo > weightFrom)
                        {
                            towers[toTower].PushToStack(towers[fromTower].PopFromStack());
                        }
                        else
                        {
                            movingBackState = true;
                            towers[fromTower].SetPositionNextLIFO(logLastMove);
                        }
                    }
                    else
                    {
                        towers[toTower].PushToStack(towers[fromTower].PopFromStack());
                        movingBackState = false;
                    }
                }
            }
        }
        else
        {
            movingBackState = false;
        }
    }

    private void CheckingIfWon()
    {
        if (towers[2].GetDisksCount() == 3)
        {
            ShowWinCutscene();
        }
    }

    private void CheckingIfWonTimer()
    {
        if (towers[2].GetDisksCount() == 3)
        {
            ShowWinCutscene();
        }
        else
        {
            ShowLoseCutscene();
        }
    }

    public void OpenPortal()
    {
        portal.SetActive(true);

    }

    public void ShowWinCutscene()
    {
        timer.SetActive(false);
        OpenPortal();
        winCutscene.SetActive(true);
        cameraGuide.transform.position = positionPortalView.position;
        cameraGuide.transform.rotation = positionPortalView.rotation;
        director.Play();
    }

    public void ShowLoseCutscene()
    {
        loseCutscene.SetActive(true);
        cameraGuide.transform.position = positionPortalView.position;
        cameraGuide.transform.rotation = positionPortalView.rotation;
        director.Play();
    }

    private int LocateDisk(GameObject disk)
    {
        int rta = -1;
        for(int i = 0; i<towers.Length; i++)
        {
            if (towers[i].IsDiskInTower(disk))
            {
                rta = i;
                i = towers.Length;
            }
        }
        return rta;
    }

    public void ShowDisksStacked()
    {
        foreach(TowerController tower in towers)
        {
            tower.GetDiskNames().ToString();
        }   
    }

    public void SavingLogLastMove(GameObject disk) {
        if (disk.CompareTag("Disk"))
        {
            logLastMove = disk.transform.position;
        }
    }

    public bool IsMovingBackState()
    {
        return movingBackState;
    }
}
enum State
{
    Playing,
    Active,
    Dormant
}