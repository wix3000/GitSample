using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;
using System.Xml.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PostList : MonoBehaviour
{
    public int bunchId;

    [SerializeField]
    Post postPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPostList());
    }

    IEnumerator GetPostList()
    {
        using(var request = API.GetPostsOfBunch(bunchId))
        {
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                switch (request.responseCode)
                {
                    case 404:
                        Debug.LogWarning("查無此討論串");
                        break;
                    default:
                        Debug.LogWarning($"{request.responseCode}: {request.downloadHandler.text}");
                        break;
                }
                yield break;
            }

            print(request.downloadHandler.text);
            var bunch = JsonUtility.FromJson<BunchTemplate>(request.downloadHandler.text);
            Transform content = GetComponentInChildren<ScrollRect>().content;

            foreach(var post in bunch.posts) {
                Post instance = Instantiate(postPrefab, content);
                instance.SetupByJson(post);
            }
        }
    }

    /* XML 版本
    IEnumerator GetPostList() {
        string uri = "https://wixart.ddns.net/fourm/classical/api.php";
        WWWForm form = new WWWForm();
        form.AddField("method", "getPosts");

        // using (var request = Post(uri, form)) # C# 6.0
        using (var request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                yield break;
            }

            var xml = XDocument.Parse(request.downloadHandler.text);

            if (xml.Root.Element("code").Value != "1")
            {
                Debug.LogWarning(xml.Root.Element("error").Value);
                yield break;
            }

            var posts = xml.Root.Element("posts");
            Transform content = GetComponentInChildren<ScrollRect>().content;
            foreach(var post in posts.Elements())
            {
                Post instance = Instantiate(postPrefab, content);
                instance.SetupByXml(post);
            }
        }
    }
    */
}
