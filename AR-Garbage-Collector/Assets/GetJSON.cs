using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Networking;
using SimpleJSON;
public class GetJSON : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(GetRequest("https://random-data-api.com/api/stripe/random_stripe"));
   
    }

    // Update is called once per frame
    IEnumerator GetRequest(string url)
    {
       using(UnityWebRequest webRequest = UnityWebRequest.Get(url)){
            yield return webRequest.SendWebRequest();
            var json = JSON.Parse(webRequest.downloadHandler.text);
            print(json);
            print(json["id"]);
            print(json["uid"]);
            print(json["token"]);
        }
        
    }
}
