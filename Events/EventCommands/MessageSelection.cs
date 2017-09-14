#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class MessageSelection : EventCommand
{
    public Transform Acteur;

    public string[] MessageString;
    public int[] SkippedCommands;

    [NonSerialized] private JoystickButtonList _selection;

    public override void OnUpdate()
    {
        _selection.Enabled = true;
        _selection.CheckJoystickAxis();
    }

    public void Start()
    {
        var rects = new Rect[MessageString.Length];
        var x = Camera.main.WorldToScreenPoint(Acteur.transform.position);
        for (var i = 0; i < MessageString.Length; i++)
        {
            rects[i] = new Rect(x.x, x.y + i*(float) Screen.height/(5*MessageString.Length),
                (float) Screen.width/(5*MessageString.Length), (float) Screen.height/(5*MessageString.Length));
        }
        _selection = new JoystickButtonList((byte) MessageString.Length, rects, MessageString, "", "Horizontal");
        _selection.Enabled = true;
    }

    public override void OnGraphic()
    {
        _selection.DisplayButtons();
    }

    public override bool Condition()
    {
        if (Input.GetButtonDown("circuit") || Input.GetButtonDown("cross"))
        {
            _selection.Enabled = false;
            return true;
        }
        return false;
    }

    public override int OnSuccess(int numberOfCommands)
    {
        return numberOfCommands + SkippedCommands[_selection.CurrentFocus];
    }
}