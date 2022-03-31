using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanManager : MonoBehaviour
{
    
    [SerializeField] private List<TrashCan> trashCanList= new List<TrashCan>();
    private List<TrashCan> _filledTrashCanList = new List<TrashCan>();
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        SetFilledTrashCanCount();
        Debug.Log("TrashCanCount: " + trashCanList.Count);
        Debug.Log("TrashCanFilled: " + _filledTrashCanList.Count);
    }
    public GameObject GetTask()
    {
        return trashCanList[0].gameObject;
    }
    public void AddTrashCan(TrashCan trashCan)
    {
        trashCanList.Add(trashCan);
    }

    public int GetFilledTrashCan()
    {
        return _filledTrashCanList.Count;
    }

    public void SetFilledTrashCanCount()

    {
        for (int i = _filledTrashCanList.Count; i < trashCanList.Count; i++)
        {
            if (trashCanList[i].GetIsFull() && !_filledTrashCanList.Contains(trashCanList[i]))
            {
                _filledTrashCanList.Add(trashCanList[i]);
            }
        }
    }
}
