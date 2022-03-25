using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private bool _isFull;

    public bool GetIsFull()
    {
        return _isFull;
    }

    // Start is called before the first frame update
    void Start()
    {
        _isFull = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
