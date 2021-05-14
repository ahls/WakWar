using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
public enum ClassType
{
    Warrior,
    Shooter,
    Supporter,
    Wak,
    Null
}
public enum WeaponType
{
    Axe,
    Sword,
    Shield,
    Bow,
    Gun,
    Throw,
    Blunt,
    Wand,
    Instrument
}

public class PulledWeapon
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
    [XmlElement("Esrc")]
    public string equipImage;
    [XmlElement("Psrc")]
    public string projImage;
    [XmlElement("projectileSpeed")]
    public float projSpeed;
    [XmlElement("projectileSound")]
    public string projSound;
    [XmlElement("impactSound")]
    public string impctSound;
    [XmlElement("armor")]
    public int Armor;
    [XmlElement("class")]
    public ClassType _class;
    [XmlElement("target")]
    public Faction targetFaction;
    [XmlElement("heightDelta")]
    public float heightDelta;
    [XmlElement("torq")]
    public int torque;
    [XmlElement("type")]
    public WeaponType weaponType;
}

public struct Weapon
{
    public string name;
    public int damage,AP,Armor, torque;
    public float AttackRange, AttackSpeed, AttackArea, projSpeed, heightDelta;
    public string equipImage, projImage, projSound, impctSound;
    public ClassType Class;
    public WeaponType weaponType;
    public Faction targetFaction;
    public Weapon(PulledWeapon _input)
    {
        name = _input.weaponName;
        damage = _input.AttackDamage;
        AP = _input.AP;
        Armor = _input.Armor;
        AttackRange = _input.AttackRange;
        AttackSpeed = _input.AttackSpeed;
        AttackArea = _input.AttackArea;
        projSpeed = _input.projSpeed;
        equipImage = _input.equipImage;
        projImage = _input.projImage;
        Class = _input._class;
        heightDelta = _input.heightDelta;
        torque = _input.torque;
        targetFaction = _input.targetFaction;
        weaponType = _input.weaponType;
        impctSound = _input.impctSound;
        projSound = _input.projSound;
    }
}
public class Weapons
{
    public static Dictionary<int, Weapon> DB = new Dictionary<int, Weapon>();
    public const string attackPrefab = "AttackEffect";
}

[XmlRoot("weaponCollection")]
public class WeaponContainer
{
    [XmlArray("weapons")]
    [XmlArrayItem("weapon")]
    public List<PulledWeapon> pulledWeapons = new List<PulledWeapon>();

    public static void Load(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        XmlSerializer serializer = new XmlSerializer(typeof(WeaponContainer));

        StringReader reader = new StringReader(xml.text);

        WeaponContainer tempContainer = serializer.Deserialize(reader) as WeaponContainer;

        reader.Close();
        foreach (PulledWeapon currentWeapon in tempContainer.pulledWeapons)
        {
            Weapons.DB[currentWeapon.weaponID] = new Weapon(currentWeapon);
        }
    }
}