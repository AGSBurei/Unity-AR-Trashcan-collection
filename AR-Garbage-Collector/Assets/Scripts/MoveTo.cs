using Pathfinding;
using UnityEngine;

public enum CarState
{
    Idl,CheckMission,LaunchTour,InTour
}

public class MoveTo : MonoBehaviour
{
    [SerializeField] public int minimumTrashCan;
    [SerializeField] public bool isAlwaysOperating;
    
    public TrashCanManager trashCanManager;
    public AIDestinationSetter destination;
    private GameObject goal;
    private CarState currentState;

    void Start()
    {
        trashCanManager = FindObjectOfType<TrashCanManager>();
        destination = FindObjectOfType<AIDestinationSetter>();
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
            destination.target = null;
        }
    }
    public void CheckMission()
    {
        if (goal == null )
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
        Debug.Log("Current mission is " + goal.transform + " position");
        destination.target = goal.transform;
        currentState = CarState.InTour;
    }
}
