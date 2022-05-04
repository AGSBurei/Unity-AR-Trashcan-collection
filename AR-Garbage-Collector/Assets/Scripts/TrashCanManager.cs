using System.Collections.Generic;
using UnityEngine;

public class TrashCanManager : MonoBehaviour
{
    [SerializeField] public List<TrashCan> trashCanList= new List<TrashCan>();
    private List<TrashCan> _filledTrashCanList = new List<TrashCan>();
    // Update is called once per frame
    void Start()
    {
        // trashCanList = FindObjectsOfType<TrashCan>;
    }
    void Update()
    {
        RefreshFilledTrashCanCount();
        RefreshEmptyTrashCanCount();
    }
    public void AddTrashCan(TrashCan trashCan)
    {
        trashCanList.Add(trashCan);
    }
    public GameObject GetTask()
    {
        return _filledTrashCanList[0].gameObject;
    }
    public int GetFilledTrashCan()
    {
        return _filledTrashCanList.Count;
    }
    public void RefreshFilledTrashCanCount()
    {
        for (int i = 0; i < trashCanList.Count; i++)
        {
            if (trashCanList[i].GetIsFull() && !_filledTrashCanList.Contains(trashCanList[i]))
            {
                _filledTrashCanList.Add(trashCanList[i]);
            }
        }
    }
    public void RefreshEmptyTrashCanCount()
    {
        for (int i = 0; i < trashCanList.Count; i++)
        {
            if (!trashCanList[i].GetIsFull())
            {
                if (_filledTrashCanList.Contains(trashCanList[i]))
                {
                    _filledTrashCanList.Remove(trashCanList[i]);
                }
            }
        }
    }
    
}
