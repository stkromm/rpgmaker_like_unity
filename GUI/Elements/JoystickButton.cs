#region

using UnityEngine;

#endregion

public class JoystickButton
{
    public Texture Over, Down;
    public string Text;
    public Rect ButtonRect;
    public bool IsPressed, IsFocused, Enabled;
    private const float FlashSpeed = 3f;
    public bool Animated = true;

    public JoystickButton(Rect rect, string label)
    {
        Enabled = true;
        Text = label;
        ButtonRect = rect;
        IsPressed = false;
        IsFocused = false;
    }

    public void Display()
    {
        Over = GUI.skin.button.hover.background;
        Down = GUI.skin.button.active.background;

        if (IsFocused && !IsPressed)
        {
            GUI.DrawTexture(ButtonRect, Over);
            if (Animated)
            {
                GUI.skin.button.normal.textColor = Color.Lerp(Color.white, Color.magenta,
                    (Mathf.Sin(Time.realtimeSinceStartup * FlashSpeed) + 1) / 2.0f);
            }
            else
            {
                GUI.skin.label.normal.textColor = GUI.skin.button.hover.textColor;
            }
            GUI.Button(ButtonRect, Text);
        }
        else if (IsFocused && IsPressed)
        {
            GUI.DrawTexture(ButtonRect, Down);
            GUI.skin.button.normal.textColor = GUI.skin.button.hover.textColor;
            GUI.Button(ButtonRect, Text);
        }
        else if (!IsFocused)
        {
            GUI.skin.button.normal.textColor = GUI.skin.button.hover.textColor;
            GUI.Button(ButtonRect, Text);
            UnClick();
        }
    }

    public void Focus(bool fo)
    {
        IsFocused = fo;
    }

    public void Focus()
    {
        IsFocused = true;
    }

    public bool Click()
    {
        if (IsFocused)
        {
            IsPressed = true;
            return true;
        }
        return false;
    }

    public void UnClick()
    {
        if (IsPressed)
        {
            IsPressed = false;
        }
    }
}