using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;

public class Post : MonoBehaviour
{
    public Text headerLabel, contentLabel, timeLabel;
    public Button imageButton;
    string imageName;

    [SerializeField]
    ImageView imageViewPrefab;

    public void SetupByXml(XElement xml) {
        string title = xml.Element("title").Value;
        string author = xml.Element("author").Value;
        string id = xml.Attribute("id").Value;

        headerLabel.text = $"{title} - {author} <color=#00F>#{id}</color>";
        contentLabel.text = xml.Element("content").Value;
        timeLabel.text = xml.Element("created_at").Value;

        imageName = xml.Element("img_name").Value;
        imageButton.gameObject.SetActive(!string.IsNullOrEmpty(imageName));
    }

    public void SetupByJson(PostTemplate json)
    {
        string title = json.title;
        string author = json.author;
        string id = json.id.ToString();

        headerLabel.text = $"{title} - {author} <color=#00F>#{id}</color>";
        contentLabel.text = json.content;
        timeLabel.text = json.created_at;

        imageName = json.img_name;
        imageButton.gameObject.SetActive(!string.IsNullOrEmpty(imageName));
    }

    public void OnImageClick()
    {
        ImageView instance = Instantiate(imageViewPrefab);
        instance.imageName = this.imageName;
    }
}
