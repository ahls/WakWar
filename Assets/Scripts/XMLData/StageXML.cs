using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class StageData
{
    [XmlElement("id")]
    public int StageID;
    [XmlElement("floor")]
    public int Floor;
    [XmlElement("room")]
    public int Room;
    [XmlArray("enemyList")]
    [XmlArrayItem("enemyData")]
    public List<EnemyData> EnemyDatas = new List<EnemyData>();
}

[XmlType("enemyData")]
public class EnemyData
{
    [XmlElement("id")]
    public int EnemyID;
    [XmlElement("position_x")]
    public float Position_x;
    [XmlElement("position_y")]
    public float Position_y;
}

public class Stages
{
    public static Dictionary<int, StageData> DB = new Dictionary<int, StageData>();
}
[XmlRoot("StageData")]
[XmlInclude(typeof(EnemyData))]
public class StageXML 
{
    [XmlArray("Stages")]
    [XmlArrayItem("Stage")]
    public List<StageData> stageDatas = new List<StageData>();

    public static void Load(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(StageXML));

        StringReader reader = new StringReader(xml.text);

        StageXML instance = serializer.Deserialize(reader) as StageXML;
        reader.Close();
        foreach (var item in instance.stageDatas)
        {
            Stages.DB[item.StageID] = item;
        }
    }
}
