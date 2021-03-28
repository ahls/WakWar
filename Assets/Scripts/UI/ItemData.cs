using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
public class Item
{
    [XmlElement("id")]
    public int itemID;
    [XmlElement("type")]
    public ItemType itemType;
    [XmlElement("name")]
    public string name;
    [XmlElement("desc")]
    public string desc;
    [XmlElement("value")]
    public int value;
    [XmlElement("weaponID")]
    public int weaponID;
}
public class ItemDB
{
    public static ItemDB instance;
    [XmlArray("items")]
    [XmlArrayItem("item")]
    public List<Item> weapons = new List<Item>();

    public static void Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        Debug.Log(_xml);
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDB));

        StringReader reader = new StringReader(_xml.text);

        instance = serializer.Deserialize(reader) as ItemDB;

        reader.Close();

    }
}
