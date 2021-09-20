using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class DialogueData
{
    [XmlElement("id")]
    public int DialogueID;//XXYY, XX: 층, YY: 순서
    [XmlArray("dialogueList")]
    [XmlArrayItem("dlg")]
    public List<Dialogue> DialogueList = new List<Dialogue>();
}

[XmlType("dialogue")]
public class Dialogue
{
    [XmlElement("portraitImage")]
    public string PortraitImage;//말풍선 꼬리 위치
    [XmlElement("name")]
    public string NameEntry;
    [XmlElement("text")]
    public string TextEntry;
}

public class Dialogues
{
    public static Dictionary<int, List<Dialogue>> DB = new Dictionary<int, List<Dialogue>>();
    
}

[XmlRoot("DialogueData")]
[XmlInclude(typeof(Dialogue))]
public class DialogueXML 
{
    [XmlArray("Dialogues")]
    [XmlArrayItem("Dialogue")]
    public List<DialogueData> DialogueDatas = new List<DialogueData>();

    public static void Load(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueXML));

        StringReader reader = new StringReader(xml.text);

        DialogueXML instance = serializer.Deserialize(reader) as DialogueXML;
        reader.Close();

        foreach (var pulledDialogue in instance.DialogueDatas)
        {
            Dialogues.DB[pulledDialogue.DialogueID] = pulledDialogue.DialogueList;
        }
    }
}
