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

    // Update is called once per frame
    void Update()
    {
        
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
            Debug.Log("Wilco here the way" + p.vectorPath);
            targetPath = p;
            
        }
    }
}
