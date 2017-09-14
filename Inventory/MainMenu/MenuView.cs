#region

using UnityEngine;

#endregion

public class MenuView : MyMonoBehaviour
{
    //GameObjects
    private MenuModell _menuModell;

    // Inspector assigned
    public Texture BackgroundTexture;

    public Texture ArmorSkin;
    public GUISkin Skin;

    //screen extends variables
    private readonly int _scWi = Screen.width;

    private readonly int _scHi = Screen.height;

    //
    private readonly Language _lang = new Language();

    private void Start()
    {
        if (_menuModell == null)
        {
            var o = GameObject.Find("Inventory");
            _menuModell = o.GetSafeComponent<MenuModell>();
        }
        enabled = true;
    }

    private void OnGUI()
    {
        if (!enabled) return;
        GUI.skin = Skin;
        GUI.backgroundColor = Color.gray;
        if (_menuModell.MenuState != MenuState.Main && _menuModell.MenuState != MenuState.Nothing)
        {
            if (BackgroundTexture != null)
                GUI.DrawTexture(new Rect(0, 0, _scWi, _scHi), BackgroundTexture, ScaleMode.ScaleAndCrop);
        }
        switch (_menuModell.MenuState)
        {
            case MenuState.Main:
                MainMenu();
                break;

            case MenuState.Character:
                CharacterMenu();
                break;

            case MenuState.Achievments:
                AchievmentMenu();
                break;

            case MenuState.Tutorial:
                SaveMenu();
                break;

            case MenuState.Settings:
                SettingsMenu();
                break;

            case MenuState.Exit:
                ExitMenu();
                break;
        }
    }

    private void AchievmentMenu()
    {
        GUI.Label(new Rect(_scWi / 3f, _scHi / 3 + _scHi / 10, Screen.width / 3f, Screen.height / 20f), "TO-DO");
    }

    private void MainMenu()
    {
        _menuModell.MainMenu.DisplayButtons();
    }

    private void ExitMenu()
    {
        /* Back to Title
        GUI.Label(new Rect(_scWi / 2 - 200, _scHi / 7 * 2, 400, 80), "<size=40>Back to Title?</size>");
        if (Input.GetButtonDown("cross"))
        {
            // Resume the game
            ResumeGame();
            String[] objectNames = { "Inventory", "GroupParty", "Hero", "Main Camera", "Game Starter" };
            GameObject o;
            // Destroy all objects, that wont be destroyed through Scene change
            foreach (String s in objectNames)
            {
                o = GameObject.Find(s);
                Destroy(o);
            }
            // Destroy this object ( otherwise some strange gui overlapping happens)
            Destroy(this);
            // Load the Title Scene
            Application.LoadLevel(1);
        }*/
    }

    private void SettingsMenu()
    {
        GUI.Label(new Rect(_scWi / 3f, _scHi / 3 + _scHi / 10, Screen.width / 3f, Screen.height / 20f), "TO-DO");
    }

    private void CharacterMenu()
    {
        CharacterMenuBottom();
        _menuModell.CharacterMenu.DisplayButtons();
        if (_menuModell.CharacterToolBarInt == 1)
        {
            CharacterViewUi();
            _menuModell.ArmorMenu.DisplayButtons();
            EquipableItemMenu();
        }
        else
        {
            GUI.Box(new Rect(0, _scHi / 20, _scWi / 5 * 4, _scHi - _scHi / 20 - _scHi / 40), "");
        }
        if (_menuModell.ItemsMenu.Enabled)
        {
            _menuModell.ItemsMenu.DisplayButtons();
        }
    }

    private void EquipableItemMenu()
    {
        if (_menuModell.EquipSlot != -1)
        {
            GUI.Box(new Rect(0, _scHi / 5f * 3f, _scWi / 5f * 4f, _scHi / 12f), "<size=30>" + _lang.ItemMenuCat[1] + "</size>");
            GUI.Box(new Rect(_scWi / 5f * 4f, _scHi / 5f * 3f, _scWi / 5f, _scHi / 5f * 2f - _scHi / 40f), "");
            GUI.Box(new Rect(0, _scHi / 5f * 3f, _scWi / 5f * 4f, _scHi / 5f * 2f - _scHi / 40f), "");
            _menuModell.ItemsMenu.DisplayButtons();
            if (_menuModell.ItemsMenu.CurrentFocus >= 0 &&
                _menuModell.ItemsMenu.CurrentFocus < _menuModell.Items.ToArray().Length)
            {
                var cha = _menuModell.Party.GetCharacterInParty(_menuModell.Party.ActiveChar);
                var s = _menuModell.AttributeChangeWithNewArmor(
                    cha.GetItemInArmorSlot(_menuModell.ArmorMenu.CurrentFocus),
                    _menuModell.Items[_menuModell.ItemsMenu.CurrentFocus].Id);
                GUI.Label(
                    new Rect(_scWi / 4f + _scWi / 7f, _scHi / 20f + _scHi / 110f, _scWi / 5f - _scWi / 20f, _scHi / 5f * 3f - _scHi / 20f),
                    s);
            }
        }
        else
        {
            GUI.Box(new Rect(0, _scHi / 5f * 3f, _scWi, _scHi / 5f * 2f - _scHi / 40f), "");
        }
    }

    private void CharacterMenuBottom()
    {
        float hours = (int)Mathf.Floor(Time.realtimeSinceStartup / 60 / 60);
        var min = Mathf.Floor(Time.realtimeSinceStartup / 60 % 60);
        var sec = Mathf.Floor(Time.realtimeSinceStartup % 60);
        _menuModell.CharacterMenuBottom.Content = "<Size=20> Played:\t" + hours + ":" + min + ":" + sec + "\t\tGold:\t" +
                                                  _menuModell.Inv.Gold + "</size>";
        _menuModell.CharacterMenuBottom.Display();
        if (_menuModell.CharacterToolBarInt == 0)
        {
            GUI.Box(new Rect(0f, _scHi / 20f, _scWi, _scHi / 20f),
                "<Size=20> Kategorie:" + _menuModell.InventoryFilter + "</size>");
        }
        if ((!_menuModell.ArmorMenu.Enabled && _menuModell.EquipSlot == -1))
        {
            if (_menuModell.ActiveCharacterBox != null)
            {
                _menuModell.ActiveCharacterBox.Display();
            }
            if (_menuModell.ItemsMenu.CurrentFocus < _menuModell.Items.ToArray().Length)
            {
                var item = _menuModell.Items.ToArray()[_menuModell.ItemsMenu.CurrentFocus];
                GUI.Box(new Rect(_scWi / 5f * 4f, _scHi / 20f, _scWi / 5f * 1f, _scHi / 5f * 3f - _scHi / 20f),
                    "<size=30>" + item.Name + "\n</size><size=22>" + item.Info + "</size>");
            }
            else
            {
                GUI.Box(new Rect(_scWi / 5f * 4f, _scHi / 20f, _scWi / 5f * 1f, _scHi / 5f * 3f - _scHi / 20f), "");
            }
        }
    }

    private void CharacterViewUi()
    {
        var cha = _menuModell.Party.GetCharacterInParty(_menuModell.Party.ActiveChar);
        GUI.Box(new Rect(0, _scHi / 20f, _scWi / 5f * 4f, _scHi / 5f * 3f - _scHi / 20f), "");
        if (cha == null)
        {
            return;
        }
        if (cha.FaceTex != null)
        {
            GUI.DrawTexture(new Rect(_scWi / 20f, _scHi / 12f, _scWi / 8f, _scHi / 4f), cha.FaceTex);
        }
        else
        {
            Debug.Log("Face Texture in Character Menu missing! Character " + cha.CharacterName);
        }
        if (ArmorSkin != null)
        {
            GUI.DrawTexture(new Rect(_scWi / 5 * 4, _scHi / 12f, _scWi / 8f, _scHi / 4f), ArmorSkin);
        }
        else
        {
            Debug.Log("Armor Overview Texture in Character Menu missing!");
        }
        // Attribute
        GUI.Box(new Rect(_scWi / 4f, _scHi / 20f, _scWi / 5f, _scHi / 5f * 3 - _scHi / 20f),
            "<size=35>" + cha.name + "\nLevel:" + cha.Level + "</size>\n\n<size=20>" +
            _menuModell.AttributesOfCharacter(cha) + "</size>");
        //
        var s = ItemDatabase.GetItemName(cha.GetItemInArmorSlot(_menuModell.ArmorMenu.CurrentFocus));
        if (s == "")
        {
            s = "Nothing equipt in this slot!";
        }
        GUI.Box(new Rect(_scWi / 5f * 4f, _scHi / 20, _scWi / 5f, _scHi / 5 * 3 - _scHi / 20),
            "<Size=20>Equipt in Slot:\n\n" + s + "\n\n" +
            ItemDatabase.GetItemInfo(cha.GetItemInArmorSlot(_menuModell.ArmorMenu.CurrentFocus)) + "</size>");
    }

    private void SaveMenu()
    {
        GUI.Label(new Rect(_scWi / 3f, _scHi / 3 + _scHi / 10, Screen.width / 3f, Screen.height / 20f), "TO-DO");
    }
}