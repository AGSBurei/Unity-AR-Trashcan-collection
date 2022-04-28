using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private Seeker seeker;

    public Path targetPath;
    // Start is called before the first frame update
    void Start()
    {
        seeker = FindObjectOfType<Seeker>();
    }
    public void CalculatePath(GameObject goal)
    {
        Path p = seeker.StartPath(transform.position, goal.transform.position, OnPathComplete);
        p.BlockUntilCalculated();
    }
    
    public void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.LogWarning("No path could be found");
        }
        else
        {
            Debug.Log("Path calculation completed");
            targetPath = p;
        }
    }
}
