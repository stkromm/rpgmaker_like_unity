public class Language
{
    private readonly string[] _settingsItems = {"Input", "Graphics", "Sound", "Interface"};

    public virtual string[] SettingsItems
    {
        get { return _settingsItems; }
    }

    private readonly string[] _menu1Items =
    {
        "Menu",
        "Achievements",
        "Tutorial",
        "Settings",
        "Exit"
    };

    public virtual string[] Menu1Items
    {
        get { return _menu1Items; }
    }

    private readonly string[] _menu2Items =
    {
        "Items", "Character", "Skills", "Buffs"
    };

    private readonly string[] _statusItems =
    {
        "HP:            ", "MP:         ", "Attack:      ", "Defense:   ", "Speed:     ", "Mind:       ", "Luck:      ",
        "Parry:     ", "Reflex:      "
    };

    public virtual string[] StatusItems
    {
        get { return _statusItems; }
    }

    public virtual string[] Menu2Items
    {
        get { return _menu2Items; }
    }

    private readonly string[] _propItems =
    {
        "Heal:        ", "Refresh:    ", "Attack:            ", "Defense:    ",
        "Speed:       ", "Mind:         ", "Luck:        ", "Parry:      ", "Reflex:      ", "ParticipantCondition:",
        "Slot:         "
    };

    public virtual string[] PropItems
    {
        get { return _propItems; }
    }

    private readonly string[] _itemMenuCat = {"Consumable", "Equipment"};

    public virtual string[] ItemMenuCat
    {
        get { return _itemMenuCat; }
    }

    private readonly string[] _inputItems =
    {
        "Sprint", "Interact", "Menu1", "Menu2", "MoveTransform forward",
        "MoveTransform backward", "MoveTransform right", "MoveTransform left"
    };

    public virtual string[] InputItems
    {
        get { return _inputItems; }
    }
}