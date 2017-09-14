#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

#endregion

public static class XmlManager
{
    public static Item[] LoadItemDatabase()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/resources/XmlDatabase/ItemDatabase.xml");
        var itemDataBase = new List<Item>();
        foreach (XmlNode item in xmlDoc.ChildNodes[1].ChildNodes)
        {
            switch (item.Name)
            {
                case "EquipableItem":
                    itemDataBase.Add((LoadXmlEquipableItem(item)));
                    break;

                case "UsableItem":
                    itemDataBase.Add((LoadXmlUsableItem(item)));
                    break;

                case "MiscItem":
                    itemDataBase.Add((LoadXmlMiscItem(item)));
                    break;
            }
        }
        return itemDataBase.ToArray();
    }

    private static EquipableItem LoadXmlEquipableItem(XmlNode item)
    {
        var entity = new EquipableItem {BuffId = -1};
        var itemInfo = item.Attributes;
        if (itemInfo != null)
            foreach (XmlAttribute a in itemInfo)
            {
                var x = 0;
                Int32.TryParse(a.Value, out x);
                switch (a.Name)
                {
                    case "name":
                        entity.Name = a.Value;
                        break;

                    case "value":
                        entity.Value = x;
                        break;

                    case "unique":
                        entity.Unique = (x == 1);
                        break;

                    case "buff":
                        entity.BuffId = x;
                        break;

                    case "slot":
                        entity.Slot = (ArmorSlot) x;
                        break;

                    case "description":
                        entity.Description = a.Value;
                        break;
                }
            }
        foreach (XmlAttribute b in item.ChildNodes[0].Attributes)
        {
            var x = 0;
            Int32.TryParse(b.Value, out x);
            switch (b.Name)
            {
                case "health":
                    entity.Stats[0] = x;
                    break;

                case "mana":
                    entity.Stats[1] = x;
                    break;

                case "attack":
                    entity.Stats[2] = x;
                    break;

                case "defense":
                    entity.Stats[3] = x;
                    break;

                case "speed":
                    entity.Stats[4] = x;
                    break;

                case "intellect":
                    entity.Stats[5] = x;
                    break;

                case "luck":
                    entity.Stats[6] = x;
                    break;

                case "parry":
                    entity.Stats[7] = x;
                    break;

                case "reflect":
                    entity.Stats[8] = x;
                    break;
            }
        }
        return entity;
    }

    private static UsableItem LoadXmlUsableItem(XmlNode item)
    {
        var entity = new UsableItem();
        var itemInfo = item.Attributes;
        if (itemInfo == null) return entity;
        foreach (XmlAttribute a in itemInfo)
        {
            var x = 0;
            Int32.TryParse(a.Value, out x);
            switch (a.Name)
            {
                case "name":
                    entity.Name = a.Value;
                    break;

                case "description":
                    entity.Description = a.Value;
                    break;

                case "value":
                    entity.Value = x;
                    break;

                case "unique":
                    entity.Unique = (x == 1);
                    break;

                case "heal":
                    entity.Heal = x;
                    break;

                case "refresh":
                    entity.Refresh = x;
                    break;

                case "buff":
                    entity.BuffId = x;
                    break;

                case "condition":
                    entity.Condition = (Condition) x;
                    break;
            }
        }
        return entity;
    }

    private static MiscItem LoadXmlMiscItem(XmlNode item)
    {
        var entity = new MiscItem();
        entity.BuffId = -1;
        var itemInfo = item.Attributes;
        if (itemInfo == null) return entity;
        foreach (XmlAttribute a in itemInfo)
        {
            var x = 0;
            Int32.TryParse(a.Value, out x);
            switch (a.Name)
            {
                case "name":
                    entity.Name = a.Value;
                    break;

                case "value":
                    entity.Value = x;
                    break;

                case "unique":
                    entity.Unique = (x == 1);
                    break;

                case "buff":
                    entity.BuffId = x;
                    break;

                case "description":
                    entity.Description = a.Value;
                    break;
            }
        }
        return entity;
    }

    public static SkillList LoadSKillDatabase()
    {
        var serializer = new XmlSerializer(typeof (SkillList));
        var loadStream = new FileStream(Application.dataPath + "/resources/XmlDatabase/SkillList.xml",
            FileMode.Open, FileAccess.Read);
        var loadedObject = (SkillList) serializer.Deserialize(loadStream);
        loadStream.Close();
        return loadedObject;
    }

    public static Buff[] LoadBuffDatabase()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/resources/XmlDatabase/BuffDatabase.xml");
        var buffDataBase = new List<Buff>();
        foreach (XmlNode item in xmlDoc.ChildNodes[1].ChildNodes)
        {
            var s = new Buff(0, "");
            foreach (XmlAttribute att in item.Attributes)
            {
                var x = 0;
                Int32.TryParse(att.Value, out x);
                switch (att.Name)
                {
                    case "Name":
                        s.Name = att.Value;
                        break;

                    case "Duration":
                        s.Duration = x;
                        break;

                    case "Health":
                        s.Stats[0] = x;
                        break;

                    case "Mana":
                        s.Stats[1] = x;
                        break;

                    case "Attack":
                        s.Stats[2] = x;
                        break;

                    case "Defense":
                        s.Stats[3] = x;
                        break;

                    case "Speed":
                        s.Stats[4] = x;
                        break;

                    case "Intellect":
                        s.Stats[5] = x;
                        break;

                    case "Parry":
                        s.Stats[6] = x;
                        break;

                    case "Luck":
                        s.Stats[7] = x;
                        break;

                    case "Reflect":
                        s.Stats[8] = x;
                        break;
                }
            }
            buffDataBase.Add(s);
        }
        return buffDataBase.ToArray();
    }
}