using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Xml.Linq;

public class PostManager : MonoBehaviour
{
    public InputField authorField, titleField, contentField;

    public Text imageNameLabel;
    public Image imagePreview;
    byte[] rawData;

    public void OnPickImageClick()
    {
        string imagePath = "";
#if UNITY_EDITOR
        imagePath = UnityEditor.EditorUtility
            .OpenFilePanelWithFilters("選擇圖片", "", new[] { "圖片檔案", "png,jpg,jpeg" });
#elif UNITY_ANDROID
#elif UNITY_IOS
#endif

        if (imagePath != "")
        {
            StartCoroutine(LoadLocalImage(imagePath));
        }
    }

    IEnumerator LoadLocalImage(string imagePath)
    {
        using (var imageRequest = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return imageRequest.SendWebRequest();
            if (imageRequest.isNetworkError)
            {
                Debug.LogWarning(imageRequest.error);
                yield break;
            }

            // 取得圖檔名稱並顯示
            imageNameLabel.text = System.IO.Path.GetFileName(imagePath);
            // 生成預覽用 Sprite
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            Sprite sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            imagePreview.sprite = sprite;
            rawData = imageRequest.downloadHandler.data;
        }
    }


    public void OnSendClick() {
        StartCoroutine(PostUploadHandler());
    }

    IEnumerator PostUploadHandler() {
        string uri = "https://wixart.ddns.net/fourm/classical/api.php";
        WWWForm form = new WWWForm();
        form.AddField("method", "uploadPost");
        form.AddField("author", authorField.text);
        form.AddField("title", titleField.text);
        form.AddField("content", contentField.text);

        if (rawData != null)
        {
            form.AddBinaryData("image", rawData);
        }

        using (UnityWebRequest request = UnityWebRequest.Post(uri, form)) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) {
                Debug.LogError(request.error);
                yield break;
            }
            
            var xml = XDocument.Parse(request.downloadHandler.text);
            string code = xml.Root.Element("code").Value;

            if (code != "1") {
                Debug.LogWarning(xml.Root.Element("error").Value);
                yield break;
            }

            Debug.Log(xml.Root.Element("message").Value);
        }
    }
}
