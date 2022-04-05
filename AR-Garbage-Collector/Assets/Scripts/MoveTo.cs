using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    private GameObject _goal;
    [SerializeField] public int minimumTrashCan;
    [SerializeField] public bool isAlwaysOperating;
    public TrashCanManager trashCanManager;
    private NavMeshAgent _agent;
    private bool collectMode;
    // Start is called before the first frame update
    void Start()
    {
        trashCanManager = FindObjectOfType<TrashCanManager>();
        _agent = GetComponent<NavMeshAgent>();
        collectMode = false;
    }
    void Update()
    {
        UpdateOperationMode();
        CheckCurrentMission();
    }
    public void UpdateOperationMode()
    {
        if (trashCanManager.GetFilledTrashCan() >= minimumTrashCan | isAlwaysOperating)
        {
            Debug.Log("Trash can collection started");
            collectMode = true;
        }
        else
        {
            Debug.Log("Trash can collection halted");
            collectMode = false;
        }
    }
    public void CheckCurrentMission()
    {
        if (_goal == null )
        {
            if (trashCanManager.GetFilledTrashCan() > 0)
            {
                GetNewMission();
            }
        }

        if (_goal != null)
        {
            if (!_goal.GetComponent<TrashCan>()._isFull)
            {
                if (trashCanManager.GetFilledTrashCan() > 0)
                {
                    GetNewMission();
                    Debug.Log("New mission assign target is now: " + _goal.gameObject.name);
                }
            }
        }
    }
    public void GetNewMission()
    {
        if (collectMode)
        {
            _goal = trashCanManager.GetTask();
            _agent.destination = _goal.transform.position;
        }
    }
}
