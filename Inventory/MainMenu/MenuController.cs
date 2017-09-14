#region

using System;
using UnityEngine;

#endregion

public class MenuController : MyMonoBehaviour
{
    //GameObjects
    public MenuModell MenuModell;

    public AudioClip Triangle;
    public AudioClip Submit;
    public AudioClip Equip;
    public AudioClip Unequip;
    public AudioClip Usable;
    public AudioClip Exit;
    public AudioClip Evade;
    public AudioSource Sound;

    //Selection State of the menu
    private readonly int _currentlyPressedButton = -1;

    private int _frameCount;

    private delegate void SimpleVoid();

    private bool _delay;
    private const float DeadTime = 0.25f;
    private float _timePassed;

    private void Start()
    {
        Submit = (AudioClip)Resources.Load("Cursor2", typeof(AudioClip));
        Exit = (AudioClip)Resources.Load("Cansel2", typeof(AudioClip));
        Evade = (AudioClip)Resources.Load("Cursor1", typeof(AudioClip));
        Equip = (AudioClip)Resources.Load("equip", typeof(AudioClip));
        Unequip = (AudioClip)Resources.Load("unequip", typeof(AudioClip));
        Usable = (AudioClip)Resources.Load("usable", typeof(AudioClip));
        Sound.clip = Submit;
    }

    private void Update()
    {
        if (MenuModell == null) return;
        PressedTriangle();
        if (MenuModell.MenuState == MenuState.Nothing) return;
        PressedCircuit();
        RecentMenuButtonsPressed();
    }

    private void RecentMenuButtonsPressed()
    {
        if (MenuModell.CharacterMenu.Enabled)
        {
            if (Input.GetButtonDown("l1"))
            {
                Sound.clip = Evade;
                Sound.Play();
                MenuModell.ArmorMenu.Enabled = false;
                MenuModell.ItemsMenu.Enabled = false;
                MenuModell.CharacterMenu.SetFocus(1);
                MenuModell.EquipSlot = -1;
            }
            if (Input.GetButtonDown("r1"))
            {
                Sound.clip = Evade;
                Sound.Play();
                MenuModell.ArmorMenu.Enabled = false;
                MenuModell.ItemsMenu.Enabled = false;
                MenuModell.CharacterMenu.SetFocus(-1);
                MenuModell.EquipSlot = -1;
            }
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (MenuModell.CharacterToolBarInt != MenuModell.CharacterMenu.CurrentFocus)
            {
                MenuModell.CharacterToolBarInt = MenuModell.CharacterMenu.CurrentFocus;
            }
            switch (MenuModell.CharacterToolBarInt)
            {
                case 0:
                    CharacterMenuButtonPressed(MenuModell.ItemsMenu, MenuModell.InventoryUiSetup);
                    MenuModell.ItemsMenu.Enabled = true;
                    break;

                case 1:
                    CharacterMenuButtonPressed(MenuModell.ItemsMenu, MenuModell.EquipableItemOfSlotSetup);
                    if (MenuModell.EquipSlot == -1)
                    {
                        MenuModell.ArmorMenu.Enabled = true;
                        MenuModell.ItemsMenu.Enabled = false;
                    }
                    else
                    {
                        MenuModell.ItemsMenu.Enabled = true;
                    }
                    break;

                case 2:
                    CharacterMenuButtonPressed(MenuModell.ItemsMenu, MenuModell.SkillViewUiSetup);
                    MenuModell.ItemsMenu.Enabled = true;
                    break;

                case 3:
                    CharacterMenuButtonPressed(MenuModell.ItemsMenu, MenuModell.BuffViewUiSetup);
                    MenuModell.ItemsMenu.Enabled = true;
                    break;
            }
        }
        if (MenuModell.MainMenu.Enabled)
        {
            MenuModell.MainMenu.CheckInput(MainMenuButtonPressed, FocusChangeSound);
        }
        else if (MenuModell.ItemsMenu.Enabled)
        {
            MenuModell.ItemsMenu.CheckInput(ItemListButtonPressed, FocusChangeSound);
            if (MenuModell.CharacterToolBarInt == 0)
            {
                InventoryButtons();
            }
        }
        else if (MenuModell.ArmorMenu.Enabled)
        {
            MenuModell.ArmorMenu.CheckInput(ArmorMenuButtonPressed, FocusChangeSound);
            if (!Input.GetButtonDown("square")) return;
            Sound.clip = Unequip;
            Sound.Play();
            MenuModell.Party.GetCharacterInParty(MenuModell.Party.ActiveChar)
                .EquipSlotWithItem(MenuModell.ArmorMenu.CurrentFocus + 1, -1);
        }
    }

    private void InventoryButtons()
    {
        var lenght = Enum.GetNames(typeof(ItemFilter)).Length;
        if (Input.GetButtonDown("square"))
        {
            MenuModell.InventoryFilter = (int)MenuModell.InventoryFilter + 1 >= lenght
                ? 0
                : MenuModell.InventoryFilter + 1;
            MenuModell.ItemsMenu.Enabled = false;
            switch (MenuModell.InventoryFilter)
            {
                case ItemFilter.Equip:

                    Sound.clip = Equip;
                    break;

                case ItemFilter.Usable:
                    Sound.clip = Usable;
                    break;

                default:
                    Sound.clip = Evade;
                    break;
            }
            Sound.Play();
        }
        var axis = Input.GetAxis("AnalogX");
        _delay = Time.realtimeSinceStartup - _timePassed > DeadTime ? true : false;
        if (_delay && axis != 0f)
        {
            Sound.clip = Evade;
            Sound.Play();
            if (axis > 0)
            {
                MenuModell.InventoryFilter = (int)MenuModell.InventoryFilter + 1 >= lenght
                    ? 0
                    : MenuModell.InventoryFilter + 1;
            }
            else
            {
                MenuModell.InventoryFilter = (int)MenuModell.InventoryFilter - 1 < 0
                    ? (ItemFilter)lenght - 1
                    : MenuModell.InventoryFilter - 1;
            }
            _delay = false;
            _timePassed = Time.realtimeSinceStartup;
            MenuModell.ItemsMenu.Enabled = false;
        }
    }

    private void PressedTriangle()
    {
        if (!Input.GetButtonDown("triangle")) return;
        if (MenuModell.MenuState == MenuState.Nothing)
        {
            Sound.clip = Evade;
            Sound.Play();
            MenuModell.MenuState = MenuState.Main;
            MenuModell.MainMenu.Enabled = true;
            MenuModell.StopGame();
        }
        else
        {
            Sound.clip = Evade;
            Sound.Play();
            MenuModell.Party.ChangeActiveCharacter();
            MenuModell.CreateActiveCharacterBox();
            MenuModell.ArmorMenu.Enabled = false;
            MenuModell.ItemsMenu.Enabled = false;
            MenuModell.EquipSlot = -1;
        }
    }

    private void PressedCircuit()
    {
        if (!Input.GetButtonDown("circuit")) return;
        Sound.clip = Exit;
        Sound.Play();
        switch (MenuModell.MenuState)
        {
            // Menu decall
            case MenuState.Character:
                if (MenuModell.EquipSlot != -1)
                {
                    MenuModell.EquipSlot = -1;
                    MenuModell.ArmorMenu.Enabled = true;
                    MenuModell.ItemsMenu.Enabled = false;
                }
                else
                {
                    MenuModell.MenuState = MenuState.Main;
                    MenuModell.MainMenu.Enabled = true;
                }
                break;

            case MenuState.Main:
                MenuModell.MenuState = MenuState.Nothing;
                MenuModell.CharacterMenu.Enabled = false;
                MenuModell.ItemsMenu.Enabled = false;
                MenuModell.ArmorMenu.Enabled = false;
                MenuModell.ResumeGame();
                break;
            // Menu call
            default:
                MenuModell.MenuState = MenuState.Main;
                MenuModell.MainMenu.Enabled = true;
                MenuModell.StopGame();
                break;
        }
    }

    private void MainMenuButtonPressed()
    {
        Sound.clip = Submit;
        Sound.Play();
        MenuModell.MenuState = (MenuState)MenuModell.MainMenu.CurrentFocus;
        MenuModell.MainMenu.Enabled = false;
        switch (MenuModell.MenuState)
        {
            case MenuState.Main:
                Debug.Log("Main");
                MenuModell.MainMenu.Enabled = true;
                MenuModell.CharacterMenu.Enabled = false;
                MenuModell.ArmorMenu.Enabled = false;
                break;

            case MenuState.Character:
                Debug.Log("Character");
                MenuModell.MainMenu.Enabled = false;
                MenuModell.CharacterMenu.Enabled = true;
                MenuModell.CreateCharacterMenuBottom();
                break;

            default:
                MenuModell.MainMenu.Enabled = false;
                break;
        }
    }

    private void ArmorMenuButtonPressed()
    {
        Sound.clip = Submit;
        Sound.Play();
        MenuModell.EquipSlot = _currentlyPressedButton;
        MenuModell.ArmorMenu.Enabled = false;
        MenuModell.ItemsMenu.Enabled = false;
    }

    private void FocusChangeSound()
    {
        Sound.clip = Evade;
        Sound.Play();
    }

    private void ItemListButtonPressed()
    {
        if (!MenuModell.Items.ToArray()[MenuModell.ItemsMenu.CurrentFocus].Use())
        {
            Sound.clip = Exit;
            Sound.Play();
            MenuModell.Announcer.AddAnnouncement("It's not the right time for this.");
        }
        else
        {
            Sound.clip = Submit;
            Sound.Play();
            MenuModell.ItemsMenu.Enabled = false;
        }
    }

    private void CharacterMenuButtonPressed(JoystickButtonList menu, SimpleVoid view)
    {
        MenuModell.ArmorMenu.Enabled = false;
        if (menu.Enabled) return;
        view();
        menu.Enabled = true;
    }
}