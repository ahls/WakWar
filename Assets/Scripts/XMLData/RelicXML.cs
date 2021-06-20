using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class RelicData
{
    [XmlElement("id")]
    public string RelicID;
    [XmlElement("class")]
    public int Classes;
    [XmlArray("relicEffects")]
    [XmlArrayItem("relicEffect")]
    public List<RelicEffect> RelicEffects = new List<RelicEffect>();
}

[XmlType("relicEffect")]
public class RelicEffect
{
    [XmlElement("effectType")]
    public string effectType;
    [XmlElement("value")]
    public float value;
}

public class Relics
{
    public static Dictionary<string, List<RelicEffect>> DB = new Dictionary<string, List<RelicEffect>>();
    
}

[XmlRoot("RelicData")]
[XmlInclude(typeof(RelicEffect))]
public class RelicXML 
{
    [XmlArray("Relics")]
    [XmlArrayItem("Relic")]
    public List<RelicData> RelicDatas = new List<RelicData>();

    public static void Load(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(RelicXML));

        StringReader reader = new StringReader(xml.text);

        RelicXML instance = serializer.Deserialize(reader) as RelicXML;
        reader.Close();

        foreach (var pulledRelic in instance.RelicDatas)
        {
            Relics.DB[pulledRelic.RelicID] = pulledRelic.RelicEffects;
        }
    }
}
