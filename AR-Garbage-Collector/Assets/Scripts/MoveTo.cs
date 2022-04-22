using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public enum CarState
{
    Idl,CheckMission,LaunchTour,StartMove,InTour
}

public class MoveTo : MonoBehaviour
{
    [SerializeField] public int minimumTrashCan;
    [SerializeField] public bool isAlwaysOperating;
    [SerializeField] public float Speed;

    public TrashCanManager trashCanManager;
    
    private GameObject goal;
    private Agent agent;
    private CarState currentState;
    private CharacterController characterController;
    

    void Start()
    {
        trashCanManager = FindObjectOfType<TrashCanManager>();
        agent = FindObjectOfType<Agent>();
        characterController = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        switch (currentState)
        {
            case CarState.Idl:
                UpdateOperationMode();
                break;
            case CarState.CheckMission:
                CheckMission();
                break;
            case CarState.LaunchTour:
                SetNewMission();
                break;
            case CarState.StartMove:
                StartCoroutine(MoveCar());
                break;
            case CarState.InTour:
                CheckMission();
                break;
        }
    }

    public void UpdateOperationMode()
    {
        if (trashCanManager.GetFilledTrashCan() >= minimumTrashCan | isAlwaysOperating)
        {
            Debug.Log("Trash can collection started");
            CheckMission();
        }
        else
        {
            Debug.Log("Trash can collection halted");
            goal = null;
        }
    }

    public void CheckMission()
    {
        if (goal == null)
        {
            if (trashCanManager.GetFilledTrashCan() > 0)
            {
                goal = trashCanManager.GetTask();
                Debug.Log("New mission assign target is now: " + goal.gameObject.name);
                currentState = CarState.LaunchTour;
            }
        }

        if (goal != null)
        {
            if (!goal.GetComponent<TrashCan>()._isFull)
            {
                if (trashCanManager.GetFilledTrashCan() > 0)
                {
                    goal = trashCanManager.GetTask();
                    Debug.Log("New mission assign target is now: " + goal.gameObject.name);
                    currentState = CarState.LaunchTour;
                }
            }
        }
    }

    public void SetNewMission()
    {
        Debug.Log("Current mission is " + goal.transform.position + " position");
        agent.CalculatePath(goal);
        currentState = CarState.StartMove;
    }

     public IEnumerator MoveCar()
    {
        currentState = CarState.InTour;
        var p = agent.targetPath;
        var i = 0;
        Debug.Log("Moving to target");
        Debug.Log("Vector count :" + p.vectorPath.Count);
        while (i < p.vectorPath.Count)
        {
            Debug.Log("Current waypoint is : "+ i);
            var target = (Vector3)p.path[i].position;
            characterController.transform.position = Vector3.MoveTowards(agent.transform.position,target, Speed);
            if (Vector3.Distance(agent.transform.position, target) < 2)
            {
                i++;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}