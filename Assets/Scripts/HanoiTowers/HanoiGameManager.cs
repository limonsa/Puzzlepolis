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

    PlayableDirector director;

    private bool settingUpState = true;
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
        CountDown.TimerEnding += CheckingIfWon;

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
    }
    private void Awake()
    {
        portal.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShowDisksStacked();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("\n");
        }else if (Input.GetKeyDown(KeyCode.F))
        {
            ShowWinCutscene();
        }
    }

    private void MakingMove(GameObject disk, int toTower, string source)
    {
        int fromTower = LocateDisk(disk);
        float weightTo = 0, weightFrom = 0;
        if (!movingBackState || source.Equals("TowerController")) { 
            //Debug.Log($"MakingMove: about to move Disk{disk.name} that is currently in Tower{fromTower}");
            if (fromTower >= 0)
            {
                //Debug.Log($"towers[{fromTower}].IsNextLIFO({disk.name}) is {towers[fromTower].IsNextLIFO(disk)}");
                //Debug.Log($"!towers[{toTower}].IsDiskInTower({disk.name}) is {!towers[toTower].IsDiskInTower(disk)}");
                if (towers[fromTower].IsNextLIFO(disk) && !towers[toTower].IsDiskInTower(disk))
                {
                    weightFrom = towers[fromTower].GetWeightNextLIFO();
                    weightTo = towers[toTower].GetWeightNextLIFO();
                    if(weightTo > 0) {
                        //Debug.Log($"Comparing IF weightTo:{weightTo} > weightFrom:{weightFrom} ");
                        if (weightTo > weightFrom)
                        {
                            towers[toTower].PushToStack(towers[fromTower].PopFromStack());
                        }
                        else
                        {
                            //Debug.Log($"MAKING MOVE: about to move back to {logLastMove}");
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
            //Debug.Log($"IGNORING MakingMove: Disk{disk.name} that is currently in Tower{fromTower} because is MOVING BACK to logLastPosition");
            movingBackState = false;
        }
    }

    private void CheckingIfWon()
    {
        //Debug.Log($"ChekingIfWon: towers[2].GetDisksCount() = {towers[2].GetDisksCount()}");
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
        //guide.transform.rotation = positionPortalView.rotation;
        director.Play();
    }

    public void ShowLoseCutscene()
    {
        loseCutscene.SetActive(true);
        cameraGuide.transform.position = positionPortalView.position;
        cameraGuide.transform.rotation = positionPortalView.rotation;
        //guide.transform.rotation = positionPortalView.rotation;
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
            //Debug.Log($"\nTOWER #{tower.GetTowerNumber()} says: {tower.GetDiskNames()}");
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