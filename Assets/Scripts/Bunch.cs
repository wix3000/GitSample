using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bunch : MonoBehaviour
{
    public Text subjectLabel, timstampLabel, replyCountLabel;
    public BunchTemplate json { get; private set; }

    [SerializeField]
    PostList postlistPrefab;

    public void SetupByJson(BunchTemplate json)
    {
        this.json = json;
        subjectLabel.text = json.subject;
        timstampLabel.text = json.updated_at;
        replyCountLabel.text = json.reply_count.ToString();
    }

    public void OnClick()
    {
        PostList instance = Instantiate(postlistPrefab);
        instance.bunchId = json.id;
    }
}

[System.Serializable]
public struct BunchTemplate {
    public int id;
    public string subject;
    public string updated_at;
    public string created_at;
    public int reply_count;
    public PostTemplate[] posts;
}

[System.Serializable]
public struct PostTemplate {
    public int id;
    public int u_id;
    public string author;
    public string title;
    public string content;
    public string img_name;
    public int b_id;
    public string created_at;
}
