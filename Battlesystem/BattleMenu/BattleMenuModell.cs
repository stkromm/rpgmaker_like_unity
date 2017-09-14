#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

public enum SelectionState
{
    ActionMenu = 0,
    TacticSelection = 1,
    SkillSelection = 2,
    ItemSelection = 3,
    TargetSelection = 4
}

public class BattleMenuModell : MyMonoBehaviour
{
    [HideInInspector]
    public SelectionState Selection = SelectionState.ActionMenu;

    public JoystickButtonList ChooseMenu = new JoystickButtonList(0, new Rect[0], new string[0], "", "Vertical");

    [HideInInspector]
    public BattleController Engine;

    public int ItemListIndex;

    [HideInInspector]
    public List<ListableMenuItem> Items;

    public SelectionState LastSelectedMenu = SelectionState.ActionMenu;

    [HideInInspector]
    public byte LastUseOfitemsMenu;

    [HideInInspector]
    public JoystickButtonList MainMenu = new JoystickButtonList(0, new Rect[0], new string[0],
        "", "Vertical");

    public List<GameObject> ParticipantsModells;

    [HideInInspector]
    public short Skill;

    [HideInInspector]
    public byte Target;

    public JoystickButtonList TargetMenu = new JoystickButtonList(0, new Rect[0], new string[0], "", "Vertical");

    private readonly string[] _menu = { "Attack", "Taktik", "Skills", "Items" };

    //
    private readonly int _scHi = Screen.height;

    private readonly int _scWi = Screen.width;

    [HideInInspector]
    public AnimatedValueCounter[] CharactersHealth { private set; get; }

    [HideInInspector]
    public AnimatedValueCounter[] CharactersMana { private set; get; }

    [HideInInspector]
    public Inventory Inventory { private set; get; }

    [HideInInspector]
    public Party Party { private set; get; }

    #region MonoBehaviour

    // Use this for initialization
    private void Start()
    {
        var o = GameObject.Find("Inventory");
        Inventory = o.GetComponent<Inventory>();
        Engine = GameObject.Find("BattleEngine").GetComponent<BattleController>();
        CharactersHealth = new AnimatedValueCounter[Engine.AlliedTeam.ToArray().Length];
        CharactersMana = new AnimatedValueCounter[Engine.AlliedTeam.ToArray().Length];
        for (var i = 0; i < Engine.AlliedTeam.ToArray().Length; i++)
        {
            var x = (PlayerCombatant)Engine.AlliedTeam.ToArray()[i];
            GuiElementAnimation anim =
                new FlyInGuiElementAnimation(new Rect(_scWi / 10f * 7f, _scHi / 20f * (16 + i), _scWi / 10f, _scHi / 20f),
                    Color.white, 3, 1);
            CharactersHealth[i] = new AnimatedValueCounter(x.Health, (int)x.Ch.Attributes[0].GetValueCap(x.Ch.Level),
                "HP", true, true, anim);
            anim = new FlyInGuiElementAnimation(new Rect(_scWi / 10f * 8f, _scHi / 20f * (16 + i), _scWi / 10f, _scHi / 20f),
                Color.white, 3, 1);
            CharactersMana[i] = new AnimatedValueCounter(x.Mana, (int)x.Ch.Attributes[1].GetValueCap(x.Ch.Level), "MP",
                true, true, anim);
        }
        var rect = new Rect[_menu.Length];
        for (var i = 0; i < _menu.Length; i++)
        {
            rect[i] = new Rect(0, _scHi / 15 * (15 - _menu.Length + i), _scWi / 5f, _scHi / 15f);
        }
        MainMenu = new JoystickButtonList((byte)_menu.Length, rect, _menu, "cross", "Vertical");
        ChooseMenu = new JoystickButtonList(0, new Rect[0], new string[0], "", "Vertical");
        TargetMenu = new JoystickButtonList(0, new Rect[0], new string[0], "", "Vertical");
    }

    private void Update()
    {
        switch (Selection)
        {
            case SelectionState.ActionMenu:
                MainMenu.Enabled = true;
                ChooseMenu.Enabled = false;
                TargetMenu.Enabled = false;
                break;

            case SelectionState.ItemSelection:
                MainMenu.Enabled = false;
                ChooseMenu.Enabled = true;
                TargetMenu.Enabled = false;
                SetupItemMenu();
                break;

            case SelectionState.SkillSelection:
                MainMenu.Enabled = false;
                ChooseMenu.Enabled = true;
                TargetMenu.Enabled = false;
                SetupSkillMenu();
                break;

            case SelectionState.TacticSelection:
                MainMenu.Enabled = false;
                ChooseMenu.Enabled = true;
                TargetMenu.Enabled = false;
                SetupTacticMenu();
                break;

            case SelectionState.TargetSelection:
                MainMenu.Enabled = false;
                ChooseMenu.Enabled = false;
                TargetMenu.Enabled = true;

                break;
        }
    }

    #endregion MonoBehaviour

    #region GUI Setup Methods

    public void SetupItemMenu()
    {
        Items = new List<ListableMenuItem>();
        var query = (from item in Inventory.GetItemListKeys
                     let temp = Array.IndexOf(Inventory.GetItemListKeys, item)
                     where Inventory.FilterItem(ItemFilter.Usable, item)
                     select new ListableMenuItem(item)).ToList();
        ItemListIndex = ItemListIndex - 4 >= query.ToArray().Length - 4 ? query.ToArray().Length - 4 : ItemListIndex;
        ItemListIndex = ItemListIndex < 0 ? 0 : ItemListIndex;
        for (var i = ItemListIndex; i < ItemListIndex + 4; i++)
        {
            if (query.ToArray().ValidIndex(i))
                Items.Add(query.ToArray()[i]);
        }
        ChooseMenu = MenuItemListUi(Items, _scWi / 5f, _scHi / 15 * (15 - _menu.Length));
        if (Items.ToArray().Length != 0)
        {
            ChooseMenu.Enabled = true;
            LastUseOfitemsMenu = 0;
        }
    }

    public void SetupSkillMenu()
    {
        var cha = Engine.ActiveHero.Ch;
        Items = new List<ListableMenuItem>();
        var query =
            cha.Skills.Where((t, i) => cha.Skills[i].B)
                .Select(t => new ListableMenuItem(SkillDatabase.Skills()[t.I]))
                .ToList();
        for (var i = 0; i < 5; i++)
        {
            if (query.ToArray().ValidIndex(i))
            {
                Items.Add(query.ToArray()[i]);
            }
        }
        ChooseMenu = MenuItemListUi(Items, _scWi / 5f, _scHi / 15 * (15 - _menu.Length));
        ChooseMenu.Enabled = true;
        if (Items.ToArray().Length != 0)
        {
            ChooseMenu.Enabled = true;
            LastUseOfitemsMenu = 2;
        }
    }

    public void SetupTacticMenu()
    {
    }

    public void SetupTargetSetup(BattleCombatant[] p)
    {
        var rec = new Rect[p.Length];
        for (var i = 0; i < rec.Length; i++)
        {
            rec[i] = new Rect(0, i * _scHi / 20f, _scWi / 10f, _scHi / 20f);
        }
        var names = new string[rec.Length];
        for (var i = 0; i < names.Length; i++)
        {
            names[i] = p[i].Name;
        }
        TargetMenu = new JoystickButtonList((byte)names.Length, rec, names, "cross", "Vertical") { Enabled = true };
    }

    #endregion GUI Setup Methods

    public void ChangeSelectionState(SelectionState selection)
    {
        LastSelectedMenu = Selection;
        Selection = selection;
    }

    public JoystickButtonList MenuItemListUi(IEnumerable<ListableMenuItem> list, float startX, float startY)
    {
        var rects = new List<Rect>();
        var names = new List<String>();
        var buttonWidth = _scWi / 5f;
        var buttonHeight = _scHi / 15f;
        var row = 0;
        foreach (var i in list)
        {
            rects.Add(new Rect(startX, startY + row * buttonHeight, buttonWidth,
                buttonHeight));
            names.Add("<size=20>" + i.Name + "\t" + i.AdditionalNameInfo + "</size>");
            row++;
        }
        var menu = new JoystickButtonList((byte)rects.ToArray().Length, rects.ToArray(), names.ToArray(),
            "cross", "Vertical");
        return menu;
    }

    public void UpdateSelectionState()
    {
        var temp = LastSelectedMenu;
        ChangeSelectionState(Selection);
        LastSelectedMenu = temp;
    }
}