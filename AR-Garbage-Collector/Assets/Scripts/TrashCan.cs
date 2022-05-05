using UnityEngine;


public class TrashCan : MonoBehaviour
{
    public bool _isFull;
    public string serial;

    public bool GetIsFull()
    {
        return _isFull;
    }

    // Start is called before the first frame update
    void Start()
    {
        _isFull = false;
    }

    // Update is called once per frame

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Car")
        {
            if (GetIsFull())
            {
                _isFull = false;
                Debug.Log("trashcan collector on :"+ gameObject.name);
                Debug.Log(gameObject.name + " is now: " +GetIsFull());

                
            }else Debug.Log(gameObject.name + " is already empty");

            
        }    
    }
}
