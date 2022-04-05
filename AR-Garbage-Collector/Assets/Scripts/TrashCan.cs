using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public bool _isFull;

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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Car")
        {
            _isFull = false;
            Debug.Log("trashcan collector on :"+ gameObject.name);
            Debug.Log(gameObject.name + " is now: " +GetIsFull());
        }    
    }
}
