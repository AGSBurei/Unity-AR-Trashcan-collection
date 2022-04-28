using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CallAPI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetRequest("https://random-data-api.com/api/stripe/random_stripe"));
         print("toto");
    }

    // Update is called once per frame
    IEnumerator GetRequest(string url)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(url)){
            yield return webRequest.SendWebRequest();
           

            print(webRequest.downloadHandler.text);
        }
        
    }
}
