using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
public enum WeaponType
{
    Warrior,
    Shooter,
    Supporter,
    Wak
}

public class Weapon
{
    [XmlElement("id")]
    public int weaponID;
    [XmlElement("name")]
    public string weaponName;
    [XmlElement("damage")]
    public int AttackDamage;
    [XmlElement("range")]
    public float AttackRange;
    [XmlElement("speed")]
    public float AttackSpeed;
    [XmlElement("aoe")]
    public float AttackArea;
    [XmlElement("armorPiercing")]
    public int AP;
    [XmlAttribute("Esrc")]
    public string equipImage;
    [XmlAttribute("Psrc")]
    public string projImage;
    [XmlElement("projectileSpeed")]
    public float projSpeed;
    [XmlElement("armor")]
    public int Armor;
    [XmlElement("class")]
    public WeaponType weaponType;
}


[XmlRoot("weaponCollection")]
public class WeaponDB
{
    public static WeaponDB instance;
    [XmlArray("weapons")]
    [XmlArrayItem("weapon")]
    public List<Weapon> weapons = new List<Weapon>();

    public static void Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);
        Debug.Log(_xml);
        XmlSerializer serializer = new XmlSerializer(typeof(WeaponDB));

        StringReader reader = new StringReader(_xml.text);

        instance = serializer.Deserialize(reader) as WeaponDB;

        reader.Close();

    }
}