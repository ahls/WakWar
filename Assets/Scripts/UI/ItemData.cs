﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
public class PulledItem
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

public struct Item
{
    public ItemType type;
    public string name, desc;
    public int value,weaponID;

    public Item(PulledItem _input)
    {
        type = _input.itemType;
        name = _input.name;
        desc = _input.desc;
        value = _input.value;
        if(type == ItemType.weapon)
        {
            weaponID = _input.weaponID;
        }
        else
        {
            weaponID = 0;
        }
    }
}

public class Items
{
    static public Dictionary<int, Item> DB = new Dictionary<int, Item>();
}

[XmlRoot("itemCollection")]
public class ItemContainer
{
    [XmlArray("items")]
    [XmlArrayItem("item")]
    public List<PulledItem> pulledItems = new List<PulledItem>();

    public static void Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer));

        StringReader reader = new StringReader(_xml.text);

        ItemContainer instance = serializer.Deserialize(reader) as ItemContainer;

        reader.Close();
        foreach (var _item in instance.pulledItems)
        {
            Items.DB[_item.itemID] = new Item(_item);
        }
    }
}
