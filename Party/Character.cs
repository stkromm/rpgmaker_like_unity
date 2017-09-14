#define DEBUG

#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

[Serializable]
public struct IntWithBool
{
    public int I;
    public bool B;
}

public enum ArmorSlot
{
    None,
    Weapon,
    Head,
    Chest,
    Shoes,
    Back,
    Trinket,
    Waist
}

public enum Condition
{
    Normal,
    Poision,
    Cold,
    Heat,
    Stunned,
    Blind,
    Cursed,
    Paralysed,
    Wounded,
    Dead
}

public class Character : MyMonoBehaviour
{
    #region Private Attributes

    private const int ExpCap = 1000000;
    private const int MaxLevel = 100;
    private int _exp;

    #endregion Private Attributes

    #region Inspector Assgined Public Attributes

    public int[] Armor = { -1, -1, -1, -1, -1, -1, -1, -1 };
    public Attribute[] Attributes;
    public int Level = 1;
    public string CharacterName = "";
    public Texture FaceTex;
    public IntWithBool[] Skills;
    public int Condition;
    public bool InParty;
    public bool InCombatParty;

    #endregion Inspector Assgined Public Attributes

    #region Public Attributes

    public int ExpToLevelUp = 100;
    public const int NoneArmor = -1;
    public readonly Dictionary<string, Buff> Buffs = new Dictionary<string, Buff>();
    public readonly int NumberOfArmorSlots = Enum.GetNames((typeof(ArmorSlot))).Length;

    #endregion Public Attributes

    #region Equipment

    public int GetItemInArmorSlot(int slot)
    {
        if (Armor.ValidIndex(slot))
        {
            return Armor[slot];
        }
        return NoneArmor;
    }

    public bool EquipSlotWithItem(int slot, int e)
    {
        var o = new GameObject();
        var inv = o.FindSafe("Inventory").GetSafeComponent<Inventory>();
        Debug.Log(" No anscurances, that the slot is the wished slot, when deequiping!");
        if ((ItemDatabase.GetItemSlot(e) == slot || e == NoneArmor) && DeequipItemInSlot(slot - 1, inv) && e != 0 &&
            inv.AddAmountofItem(e, NoneArmor))
        {
            Armor[slot - 1] = e;
            UpdateEquipmentStats();
            return true;
        }
        return false;
    }

    private bool DeequipItemInSlot(int piece, Inventory inv)
    {
        if (Armor[piece] != NoneArmor && !inv.AddAmountofItem(Armor[piece], 1))
        {
            return false;
        }
        Armor[piece] = NoneArmor;
        return true;
    }

    #endregion Equipment

    #region Buffs

    public bool BuffCharacter(Buff buff)
    {
        if (buff == null)
        {
#if DEBUG
            Debug.Log("Tried to buff with null-pointer Character " + CharacterName);
#endif
            return false;
        }
        if (Buffs.ContainsKey(buff.Name))
        {
            Debug.Log("Add a warning or somethin, that current Buffs can be overwritten! - Overwritten Buff " +
                      buff.Name);
            Buffs.Remove(buff.Name);
        }
        Buffs.Add(buff.Name, buff);
        UpdateBuffStats();
        return true;
    }

    public bool RefreshBuffCounter(int i)
    {
        var changed = false;
        if (i <= 0)
        {
#if DEBUG
            Debug.Log("Tried to refresh BuffCount of Character" + CharacterName + " by" + i);
#endif
            return false;
        }
        foreach (var b in Buffs.Values)
        {
            b.Duration = b.Duration - i;
            if (b.Duration <= 0)
                Buffs.Remove(b.Name);
            UpdateBuffStats();
            changed = true;
        }
        return changed;
    }

    #endregion Buffs

    #region Attributes

    private void UpdateEquipmentStats()
    {
        for (var u = 0; u < Attributes.Length; u++)
        {
            Attributes[u].AdditionalArmorStats =
                (from i in Armor where i != 0 select i).Sum(i => ItemDatabase.GetStats(i)[u]);
        }
    }

    private void UpdateBuffStats()
    {
        for (var u = 0; u < Attributes.Length; u++)
        {
            Attributes[u].AdditionalBuffStats =
                (from i in Buffs.Values where (u < i.Stats.Length && i.Stats[u] != 0) select i).Sum(b => b.Stats[u]);
        }
    }

    public bool HealCondition(Condition i)
    {
        return false;
    }

    public bool DealCondition(int i)
    {
        Debug.Log("TO-DO Condition Transitions atm everyother Condition replaceses current Condition");
        if (Condition == i)
        {
            return false;
        }
        Condition = i;
        return true;
    }

    /// <summary>
    /// Negative Values do heal.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool ChangeHealth(int i)
    {
        return Attributes[0].TakeDamage(i, Level);
    }

    /// <summary>
    /// Heal the Mana of the character by the negative value of i;
    /// </summary>
    /// <param name="i"></param>
    /// <returns>True, if the Mana of the character was changed successfully.</returns>
    public bool ChangeMana(int i)
    {
        return Attributes[1].TakeDamage(i, Level);
    }

    #endregion Attributes

    #region Level

    public void AddExp(int e)
    {
        try
        {
            _exp = _exp + e;
        }
        catch (OverflowException)
        {
#if DEBUG
            Debug.Log("Handled overflow when adding exp to" + CharacterName + " Amount" + e);
#endif
            _exp = ExpCap;
        }
        _exp = _exp > ExpCap ? ExpCap : _exp;
        while (IsLevelingUp())
        {
            Debug.Log("TO-DO Level-up Announcement");
            // Level Up Announcement
        }
    }

    private bool IsLevelingUp()
    {
        if (_exp < ExpToLevelUp)
        {
            return false;
        }
        return LevelUp();
    }

    private bool LevelUp()
    {
        if (Level >= MaxLevel)
        {
            return false;
        }
        Level++;
        _exp = _exp - ExpToLevelUp;
        ExpToLevelUp = Mathf.CeilToInt(Mathf.Exp(Level * 2) * Mathf.Exp(1f / Level));
        return true;
    }

    #endregion Level
}