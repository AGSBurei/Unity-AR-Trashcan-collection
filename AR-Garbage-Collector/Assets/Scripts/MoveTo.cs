using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    private GameObject _goal;
    public TrashCanManager trashCanManager;
    private NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        trashCanManager = FindObjectOfType<TrashCanManager>();
        _agent = GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        if (trashCanManager.GetFilledTrashCan() > 2)
        {
            _goal = trashCanManager.GetTask();
            _agent.destination = _goal.transform.position;  
        }
    }
}
