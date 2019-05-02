using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImageView : MonoBehaviour
{
    public Slider progressBar;
    public RawImage image;
    public string imageName;
    public UnityWebRequest request { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(imageName))
        {
            Destroy(gameObject);
            return;
        }

        string uri = "https://wixart.ddns.net/fourm/img/" + imageName;
        request = UnityWebRequestTexture.GetTexture(uri);
        request.SendWebRequest();
    }

    // Update is called once per frame
    void Update()
    {
        if (request == null) return;
        progressBar.value = request.downloadProgress;
        if (request.isDone)
        {
            image.texture = DownloadHandlerTexture.GetContent(request);
            image.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(false);

            Vector2 imgSize = new Vector2(image.texture.width, image.texture.height);
            Vector2 frame   = new Vector2(Screen.width, Screen.height);

            float rateW = imgSize.x / frame.x;
            float rateH = imgSize.y / frame.y;
            float rate = Mathf.Max(rateW, rateH, 1f);
            image.rectTransform.sizeDelta = imgSize / rate;

            request.Dispose();
            request = null;
        }
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
