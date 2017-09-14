#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

public class MenuModell : MyMonoBehaviour
{
    #region Attributes

    #region Public

    public int CharacterToolBarInt = 0;

    public int EquipSlot = -1;

    public Inventory Inv;

    //Selection State of the menu
    public MenuState MenuState = MenuState.Nothing;

    //GameObjects
    public Party Party;

    public float VerticalSliderValue;

    // Inspector assigned
    public Announcer Announcer;

    public AnimatedBox CharacterMenuBottom, ActiveCharacterBox, ActiveCharacterBox2;

    public ItemFilter InventoryFilter = ItemFilter.None;

    public List<ListableMenuItem> Items = new List<ListableMenuItem>();

    public JoystickButtonList MainMenu, CharacterMenu, ItemsMenu, ArmorMenu, SkillMenu, BuffMenu;

    #endregion

    #region Private

    private readonly Language _lang = new Language();

    private readonly int _scHi = Screen.height;

    //screen extends variables
    private readonly int _scWi = Screen.width;

    //None
    private readonly String[] _armorSlots = { "Weapon", "Helmet", "Chest", "Shoes", "Back", "Trinket", "Waist" };

    private int _frameCount;
    private int _lastUseOfitemsMenu = -1;

    private delegate void SimpleVoid();

    #endregion

    #endregion

    public static JoystickButtonList MenuItemListUi(IEnumerable<ListableMenuItem> list, float startX, float startY,
        byte numberOfColumns, float width, float height)
    {
        var rects = new List<Rect>();
        var names = new List<String>();
        var column = 0;
        var row = 0;
        foreach (var i in list)
        {
            rects.Add(new Rect(startX + column * width, startY + row * height, width,
                height));
            names.Add("<size=20>" + i.Name + "\t" + i.AdditionalNameInfo + "</size>");
            column = (column + 1) % numberOfColumns;
            row = column == 0 ? row + 1 : row;
        }
        var menu = new JoystickButtonList((byte)rects.ToArray().Length, rects.ToArray(), names.ToArray(),
            "cross", "Horizontal");
        return menu;
    }

    public String AttributeChangeWithNewArmor(int oldEquip, int newEquip)
    {
        var newItemStats = ItemDatabase.GetStats(newEquip);
        var oldItemStats = ItemDatabase.GetStats(oldEquip);
        var s = "<size=35>\n\n\n\n\n</size><size=20>";
        for (var a = 0; a < newItemStats.Length; a++)
        {
            var o = (newItemStats[a] - oldItemStats[a]);
            if (o == 0)
            {
                s += "<color=grey>";
            }
            else if (o > 0)
            {
                s += "<color=#008000ff>+";
            }
            else
            {
                s += "<color=#ff0000ff>";
            }

            s += o + "</color>";

            s += "\n";
        }
        s += "</size>";
        return s;
    }

    public String AttributesOfCharacter(Character cha)
    {
        var s = "";
        s = cha.Attributes.Aggregate(s,
            (current, attribute) =>
                current + _lang.StatusItems[Array.IndexOf(cha.Attributes, attribute)] + "\t" +
                attribute.GetValueCap(cha.Level) + "\\" + attribute.GetActualValue(cha.Level) + "\n");
        return s;
    }

    #region GUI Elements Setup Methods

    public void BuffViewUiSetup()
    {
        Items = new List<ListableMenuItem>();
        var cha = Party.GetCharacterInParty(Party.ActiveChar);
        Items = (from b in cha.Buffs.Values where b != null select new ListableMenuItem(b)).ToList();
        ItemsMenu = MenuItemListUi(Items, _scWi / 20f, _scHi / 12f, 3, (25f / 100f) * Screen.width, _scHi / 12f);
        _lastUseOfitemsMenu = 3;
    }

    public void CreateActiveCharacterBox()
    {
        if ((ArmorMenu.Enabled || EquipSlot != Character.NoneArmor)) return;
        var cha = Party.GetCharacterInParty(Party.ActiveChar);
        var s = AttributesOfCharacter(cha);
        GuiElementAnimation anim =
            new FlyInGuiElementAnimation(new Rect(_scWi / 5 * 4, _scHi / 5 * 3, _scWi / 5 * 1, _scHi / 5 * 2 - _scHi / 40),
                Color.white, 2f, 0.5f);
        ActiveCharacterBox =
            new AnimatedBox(
                "<size=30>" + Party.GetCharacterInParty(Party.ActiveChar).CharacterName + "\n</size><size=22>" + s +
                "</size>", true, true, anim);
    }

    public void CreateCharacterMenuBottom()
    {
        float hours = (int)Mathf.Floor(Time.realtimeSinceStartup / 60 / 60);
        var min = Mathf.Floor(Time.realtimeSinceStartup / 60 % 60);
        var sec = Mathf.Floor(Time.realtimeSinceStartup % 60);
        var time = "<Size=20> Played:\t" + hours + ":" + min + ":" + sec + "\t\tGold:\t" + Inv.Gold + "</size>";
        GuiElementAnimation anim = new FlyInGuiElementAnimation(new Rect(_scWi / 2f, _scHi / 40f * 39f, _scWi / 2f, _scHi / 39f),
            Color.white, 0f, 0.5f);
        CharacterMenuBottom = new AnimatedBox(time, true, true, anim);
    }

    public void EquipableItemOfSlotSetup()
    {
        // List of all equipable Slot Items
        int index = ItemsMenu.CurrentFocus;
        Items = new List<ListableMenuItem>();
        Items = Inv.GetItemListKeys.Where(
            i => (Inv.FilterItem((ItemFilter)ArmorMenu.CurrentFocus + 1, i)))
            .Select(i => new ListableMenuItem(i))
            .ToList();
        ItemsMenu = MenuItemListUi(Items, _scWi / 20f, _scHi / 5 * 3 + _scHi / 12f, 3, (25f / 100f) * Screen.width, _scHi / 12f);
        if (_lastUseOfitemsMenu == 1)
        {
            ItemsMenu.Enabled = true;
            for (var j = 0; j < index; j++)
            {
                ItemsMenu.SetFocus(-1);
            }
            // itemsMenu.currentFocus = index;
            ItemsMenu.Enabled = false;
        }
        _lastUseOfitemsMenu = 1;
    }

    public void InventoryUiSetup()
    {
        int index = ItemsMenu.CurrentFocus;
        Items = new List<ListableMenuItem>();
        Items = (from item in Inv.GetItemListKeys
                 let temp = Array.IndexOf(Inv.GetItemListKeys, item)
                 where /* temp >= 0 && temp <= 0 + 50 &&*/ Inv.FilterItem(InventoryFilter, item)
                 select new ListableMenuItem(item)).ToList();
        ItemsMenu = MenuItemListUi(Items, _scWi / 20f, _scHi / 10f, 3, (25f / 100f) * Screen.width, _scHi / 12f);
        if (_lastUseOfitemsMenu == 0)
        {
            ItemsMenu.Enabled = true;
            for (var j = 0; j < index; j++)
            {
                Debug.Log("Changed Focus by -1");
                ItemsMenu.SetFocus(-1);
            }
            ItemsMenu.Enabled = false;
        }
        _lastUseOfitemsMenu = 0;
    }

    public void SkillViewUiSetup()
    {
        var cha = Party.GetCharacterInParty(Party.ActiveChar);
        Items = new List<ListableMenuItem>();
        var query =
            cha.Skills.Where((t, i) => cha.Skills[i].B)
                .Select(t => new ListableMenuItem(SkillDatabase.Skills()[t.I]))
                .ToList();
        Items = query;
        ItemsMenu = MenuItemListUi(Items, _scWi / 20f, _scHi / 12f, 3, (25f / 100f) * Screen.width, _scHi / 12f);
        _lastUseOfitemsMenu = 2;
    }

    #endregion

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    #region MONOBEHAVIOUR

    private void Start()
    {
        var o = GameObject.Find("Party");
        Party = o.GetSafeComponent<Party>();
        o = GameObject.Find("Inventory");
        Inv = o.GetSafeComponent<Inventory>();
        if (Party == null || Inv == null)
            enabled = false;
        var menuRect = new Rect[_lang.Menu1Items.Length];
        //
        for (var i = 0; i < _lang.Menu1Items.Length; i++)
        {
            menuRect[i] = new Rect(_scWi / 10 * 4, _scHi / 7 * (i + 1), _scWi / 5f, _scHi / 12f);
        }
        MainMenu = new JoystickButtonList((byte)_lang.Menu1Items.Length, menuRect, _lang.Menu1Items, "cross",
            "Vertical");
        //
        ItemsMenu = new JoystickButtonList(0, new Rect[1], new String[1], "", "Horinzontal");
        //
        menuRect = new Rect[_lang.Menu2Items.Length];
        for (var i = 0; i < _lang.Menu2Items.Length; i++)
        {
            menuRect[i] = new Rect(_scWi / _lang.Menu2Items.Length * i, 0f, _scWi / (float)_lang.Menu2Items.Length, _scHi / 20f);
        }
        CharacterMenu = new JoystickButtonList((byte)_lang.Menu2Items.Length, menuRect, _lang.Menu2Items, "cross",
            "Horizontal");
        //
        menuRect = new Rect[_armorSlots.Length];
        for (var i = 0; i < _armorSlots.Length; i++)
        {
            menuRect[i] = new Rect(_scWi / 2f, _scHi / 12f + i * (_scHi / 20f * 9f / _armorSlots.Length), _scWi / 6f,
                _scHi / 20f * 9f / _armorSlots.Length);
        }
        ArmorMenu = new JoystickButtonList((byte)_armorSlots.Length, menuRect, _armorSlots, "cross", "Vertical");
        //
        InventoryUiSetup();
        CreateActiveCharacterBox();
        o = GameObject.Find("Inventory");
        var v = o.GetSafeComponent<MenuView>();
        v.enabled = true;
        var w = o.GetSafeComponent<MenuController>();
        w.enabled = true;
    }

    #endregion
}