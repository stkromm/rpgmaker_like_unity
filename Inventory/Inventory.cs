#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class Inventory : MyMonoBehaviour, IInventoryInterface
{
    //Inventory size
    private const short AmountOfItemsCap = 99;

    private const int GoldCap = 1000000;
    private readonly Dictionary<int, int> _itemList = new Dictionary<int, int>();

    public int AmountOfItems
    {
        get { return _itemList.Count; }
    }

    public int[] GetItemListKeys
    {
        get
        {
            var a = new int[AmountOfItems];
            _itemList.Keys.CopyTo(a, 0);
            return a;
        }
    }

    public int Gold { get; private set; }

    //
    public bool AddAmountofMoney(int i)
    {
        try
        {
            if (Gold + i > GoldCap || Gold + i < 0)
            {
                return false;
            }
            Gold += i;
            return true;
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    public bool AddAmountofItem(int item, int number)
    {
        if (!ContainsItem(item) && AmountOfItems == AmountOfItemsCap)
        {
            return false;
        }
        int a;
        _itemList.TryGetValue(item, out a);
        if (a + number < 0 || (a + number >= ItemDatabase.GetItemStackSize(item)))
        {
            return false;
        }
        _itemList.Remove(item);
        if (a + number != 0)
        {
            _itemList.Add(item, a + number);
        }
        return true;
    }

    public bool ContainsItem(int i)
    {
        return _itemList.ContainsKey(i);
    }

    public int GetItemCount(int i)
    {
        int a;
        _itemList.TryGetValue(i, out a);
        return a;
    }

    public bool FilterItem(ItemFilter filter, int i)
    {
        var a = ItemDatabase.GetTypeOfItem(i);
        var slot = (ArmorSlot)ItemDatabase.GetItemSlot(i);
        switch (filter)
        {
            case ItemFilter.None:
                return true;

            case ItemFilter.Shoes:
                return slot == ArmorSlot.Shoes;

            case ItemFilter.Misc:
                return a.ToString() == "MiscItem";

            case ItemFilter.Trinket:
                return slot == ArmorSlot.Trinket;

            case ItemFilter.Waist:
                return slot == ArmorSlot.Waist;

            case ItemFilter.Usable:
                return a.ToString() == "UsableItem";

            case ItemFilter.Weapon:
                return slot == ArmorSlot.Weapon;

            case ItemFilter.Head:
                return slot == ArmorSlot.Head;

            case ItemFilter.Back:
                return slot == ArmorSlot.Back;

            case ItemFilter.Chest:
                return slot == ArmorSlot.Chest;

            case ItemFilter.Equip:
                return a.ToString() == "EquipableItem";

            default:
                return true;
        }
    }

    public bool UseItem(int a, int index)
    {
        var o = GameObject.Find("Party");
        var p = o.GetSafeComponent<Party>();
        var ch = p.GetCharacterInParty(index);

        if (ItemDatabase.GetTypeOfItem(a) == typeof(EquipableItem))
        {
            ch.EquipSlotWithItem(ItemDatabase.GetItemSlot(a), a);
            return true;
        }
        if (!ContainsItem(a) || ItemDatabase.GetTypeOfItem(a) != typeof(UsableItem) ||
            (!ch.BuffCharacter(ItemDatabase.GetItemBuff(a)) &&
             !ch.ChangeHealth(-ItemDatabase.GetHeal(a)) && !ch.ChangeMana(-ItemDatabase.GetRefresh(a))))
        {
            return false;
        }
        AddAmountofItem(a, -1);
        return true;
    }
}