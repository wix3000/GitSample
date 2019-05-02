using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunchList : MonoBehaviour
{
    [SerializeField]
    Bunch bunchPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetBunchsHandle());
    }
    
    IEnumerator GetBunchsHandle()
    {
        using(var request = API.GetBunchList())
        {
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                Debug.LogWarning($"{request.responseCode}: {request.downloadHandler.text}");
                yield break;
            }

            var jsonArray = JsonArray<BunchTemplate>.FromJson(request.downloadHandler.text);
            Transform content = GetComponentInChildren<UnityEngine.UI.ScrollRect>().content;

            foreach(var bunch in jsonArray.values)
            {
                Bunch instance = Instantiate(bunchPrefab, content);
                instance.SetupByJson(bunch);
            }
        }
    }
}

public struct JsonArray<T> {
    public T[] values;

    public static JsonArray<T> FromJson(string json)
    {
        json = "{\"values\":" + json +"}";
        return JsonUtility.FromJson<JsonArray<T>>(json);
    }
}
