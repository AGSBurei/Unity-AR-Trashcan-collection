using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class Imagerecognization : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefab;
    private Dictionary<string, GameObject> prefabsList = new Dictionary<string, GameObject>();
    private ARTrackedImageManager arTrackedImageManager;

    private void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject obj in prefab)
        {
            //TODO check Vector zero
            GameObject newPrefab = Instantiate(obj, Vector3.zero, Quaternion.identity);
            newPrefab.name = obj.name;
            newPrefab.SetActive(false);
            Debug.LogWarning("object details : " + "obj name : " + obj.name + "object is : " +  newPrefab);
            prefabsList.Add(obj.name, newPrefab);
        }
    }

    public void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;

    }

    public void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(ARTrackedImage trackedImage in args.added)
        {
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in args.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in args.removed)
        {
            prefabsList[trackedImage.name].SetActive(false);
        }
    }

    public void UpdateImage(ARTrackedImage trackedImage)
    {

        string name = trackedImage.referenceImage.name;
        prefabsList[name].transform.position = trackedImage.transform.position;
        prefabsList[name].transform.rotation = trackedImage.transform.rotation;
        prefabsList[name].SetActive(true);
        Debug.LogWarning("key is : " + name);
        foreach (GameObject gameObject in prefabsList.Values)
        {
            if (gameObject.name != name)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
