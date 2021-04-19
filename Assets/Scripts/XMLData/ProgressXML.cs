using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ProgressSequence
{
    [XmlElement("eventType")]
    public CurrentEvent CurrentProgressEvent;
    [XmlElement("value")]//스테이지 불러올떄는 스테이지 아이디, 대사할때는 대사 아이디, 전투시작과 전투종료에는 암것도 없음
    public int value;
}
public class ProgressSequences
{
    public static List<ProgressSequence> DB = new List<ProgressSequence>();
}
[XmlRoot("ProgressData")]
public class ProgressXML
{
    [XmlArray("ProgressSequences")]
    [XmlArrayItem("Sequence")]
    public List<ProgressSequence> ProgressSequence = new List<ProgressSequence>();
    public static void Load(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(ProgressXML));

        StringReader reader = new StringReader(xml.text);

        ProgressXML instance = serializer.Deserialize(reader) as ProgressXML;
        ProgressSequences.DB = instance.ProgressSequence;
        reader.Close();
    }
}
