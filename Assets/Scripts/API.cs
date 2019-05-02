using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class API
{
    public const string ENTER_POINT = "https://wixart.ddns.net/fourm/restful/";

    public static UnityWebRequest Login(LoginTemplate template)
    {
        UnityWebRequest request = new UnityWebRequest(ENTER_POINT + "login", "POST");

        request.SetRequestHeader("Content-Type", "application/json");

        string json = JsonUtility.ToJson(template);
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();

        return request;
    }

    /// <summary>
    /// 生成一個用於註冊的請求實例。
    /// </summary>
    /// <param name="template">包含使用者資訊的 JSON 樣板</param>
    /// <returns>已設定好的請求</returns>
    public static UnityWebRequest SignUp(UserTemplate template)
    {
        UnityWebRequest request = new UnityWebRequest(ENTER_POINT + "users", "POST");

        request.SetRequestHeader("Content-Type", "application/json");
        string json = JsonUtility.ToJson(template);
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();

        return request;
    }

    /// <summary>
    /// 生成一個用於取得討論串清單的請求實例。
    /// </summary>
    /// <returns></returns>
    public static UnityWebRequest GetBunchList()
    {
        UnityWebRequest request = UnityWebRequest.Get(ENTER_POINT + "bunchs");
        return request;
    }

    public static UnityWebRequest GetPostsOfBunch(int bunchId)
    {
        UnityWebRequest request = UnityWebRequest.Get(ENTER_POINT + "bunchs/" + bunchId.ToString());
        return request;
    }
}
