using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class EnemyData
{
    [XmlElement("id")]
    public int EnemyID;
    [XmlElement("name")]
    public string Name;
}

public class EnemyDatas
{
    public static Dictionary<int, EnemyData> DB = new Dictionary<int, EnemyData>();
}

public class EnemyXML
{
    public List<EnemyData> enemyDatas = new List<EnemyData>();

    public static void Load(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(EnemyXML));

        StringReader reader = new StringReader(xml.text);

        EnemyXML instance = serializer.Deserialize(reader) as EnemyXML;
        reader.Close();
        foreach (var item in instance.enemyDatas)
        {
            EnemyDatas.DB[item.EnemyID] = item;
        }
    }
}
