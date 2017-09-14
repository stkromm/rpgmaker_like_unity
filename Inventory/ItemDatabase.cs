#region

using System;

#endregion

public static class ItemDatabase
{
    private static Item[] _instance;

    public static Item[] Items()
    {
        return _instance ?? (_instance = XmlManager.LoadItemDatabase());
    }

    public static string GetItemName(int i)
    {
        if (Items().ValidIndex(i))
        {
            return Items()[i].Name;
        }
        return "";
    }

    public static int GetHeal(int i)
    {
        if (!(Items().ValidIndex(i) && Items()[i] != null && (Items()[i].GetType() == typeof (UsableItem))))
        {
            return 0;
        }
        var e = (UsableItem) Items()[i];
        return e.Heal;
    }

    public static int GetRefresh(int i)
    {
        if (!(Items().ValidIndex(i) && Items()[i] != null && (Items()[i].GetType() == typeof (UsableItem))))
        {
            return 0;
        }
        var e = (UsableItem) Items()[i];
        return e.Refresh;
    }

    public static int GetItemSlot(int i)
    {
        if (!(Items().ValidIndex(i) && Items()[i] != null && (Items()[i].GetType() == typeof (EquipableItem))))
        {
            return (int) ArmorSlot.None;
        }
        var e = (EquipableItem) Items()[i];
        return (int) e.Slot;
    }

    public static int GetItemStackSize(int i)
    {
        if (i >= 0 && i < Items().Length)
            return Items()[i].Stack;
        return 0;
    }

    public static Type GetTypeOfItem(int a)
    {
        return Items()[a].GetType();
    }

    public static Buff GetItemBuff(int a)
    {
        if (Items()[a].BuffId == 0) return null;
        return BuffDatabase.Buffs()[Items()[a].BuffId];
    }

    public static string GetItemInfo(int a)
    {
        if (Items().ValidIndex(a) && Items()[a] != null)
        {
            return Items()[a].ToString();
        }
        return "";
    }

    public static int[] GetStats(int i)
    {
        if (!(Items().ValidIndex(i) && Items()[i] != null && (Items()[i].GetType() == typeof (EquipableItem))))
        {
            return new int[9];
        }
        var e = (EquipableItem) Items()[i];
        return e.Stats;
    }
}