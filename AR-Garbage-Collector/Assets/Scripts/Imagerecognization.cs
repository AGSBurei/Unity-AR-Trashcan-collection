using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class Imagerecognization : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefab;
    private Dictionary<string, GameObject> prefabsInstances = new Dictionary<string, GameObject>();
    private ARTrackedImageManager arTrackedImageManager;

    private void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject prefab in prefab)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            prefabsInstances.Add(prefab.name, newPrefab);
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
            prefabsInstances[trackedImage.name].SetActive(false);
        }
    }

    public void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject aPrefab = prefabsInstances[name];
        aPrefab.transform.position = position;
        aPrefab.SetActive(true);

        foreach (GameObject gameObject in prefabsInstances.Values)
        {
            if (gameObject.name != name)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
