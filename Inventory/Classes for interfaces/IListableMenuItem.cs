#region

using System;
using UnityEngine;

#endregion

public enum ListableMenuItemType
{
    Skill,
    Buff,
    Item
}

public class ListableMenuItem
{
    public readonly string Info;
    public readonly string Name;
    public readonly string AdditionalNameInfo;
    public readonly ListableMenuItemType Type;
    public readonly short Id;

    public ListableMenuItem(SkillListSkill s)
    {
        Info = s.ToString();
        Name = s.Name;
        Id = (short) Array.IndexOf(SkillDatabase.Skills(), s);
    }

    public ListableMenuItem(Buff b)
    {
        Info = b.ToString();
        Name = b.Name;
    }

    public ListableMenuItem(int item)
    {
        var o = GameObject.Find("Inventory");
        Id = (short) item;
        if (o == null) return;
        Info = ItemDatabase.GetItemInfo(item);
        Name = ItemDatabase.GetItemName(item);
        var inv = o.GetSafeComponent<Inventory>();
        AdditionalNameInfo = inv.GetItemCount(Id) == 1 ? "" : "Count: " + inv.GetItemCount(Id);
        Type = ListableMenuItemType.Item;
    }

    public bool Use()
    {
        switch (Type)
        {
            case ListableMenuItemType.Item:
                var o = GameObject.Find("Inventory");
                var p = GameObject.Find("Party");
                if (o == null && p == null)
                    return false;
                var party = p.GetComponent<Party>();
                return o.GetComponent<Inventory>()
                    .UseItem(Id, party.ActiveChar);

            default:
                return false;
        }
    }
}