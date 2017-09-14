#region

using UnityEngine;

#endregion

public class JoystickButtonList
{
    public string AxisName;
    public bool Enabled = false;

    private readonly byte _numberOfButtons;
    private readonly JoystickButton[] _buttons;
    public bool IsCheckingJoy;
    public byte CurrentFocus;
    private readonly string _actionButton;
    private const float DeadTime = 0.3f;
    private float _timePassed;

    public delegate void SimpleVoid();

    public void CheckInput(SimpleVoid buttonPressHandler, SimpleVoid focusChangeHandler)
    {
        int a = CurrentFocus;
        CheckJoystickAxis();
        if (a != CurrentFocus)
        {
            focusChangeHandler();
        }
        if (CheckJoystickButton() != -1)
        {
            buttonPressHandler();
        }
    }

    public JoystickButtonList(byte numOfButtons, Rect[] rectangles, string[] labels, string inputActionButton,
        string axis)
    {
        AxisName = axis;

        _numberOfButtons = numOfButtons;
        _actionButton = inputActionButton;

        _buttons = new JoystickButton[numOfButtons];
        for (var i = 0; i < numOfButtons; i++)
        {
            _buttons[i] = new JoystickButton(rectangles[i], labels[i]);
        }
        if (_buttons.Length != 0)

            _buttons[0].Focus();
        CurrentFocus = 0;
    }

    public bool CheckJoystickAxis()
    {
        IsCheckingJoy = !(Time.realtimeSinceStartup - _timePassed > DeadTime);
        if ((Mathf.Abs(Input.GetAxis(AxisName)) == 0 && Mathf.Abs(Input.GetAxis(AxisName)) == 0) || IsCheckingJoy ||
            !Enabled) return false;
        if (Input.GetAxis(AxisName) > .1f)
        {
            SetFocus(-1);
        }
        if (Input.GetAxis(AxisName) < -.1f)
        {
            SetFocus(1);
        }
        IsCheckingJoy = true;
        _timePassed = Time.realtimeSinceStartup;
        return true;
    }

    public int CheckJoystickButton()
    {
        var pressedButton = -1;
        if (Enabled)
        {
            if (Input.GetButtonDown(_actionButton))
            {
                for (var i = 0; i < _numberOfButtons; i++)
                {
                    if (_buttons[i].Click())
                    {
                        pressedButton = i;
                    }
                }
            }
            if (Input.GetButtonUp(_actionButton))
            {
                foreach (var butt in _buttons)
                {
                    butt.UnClick();
                }
            }
        }
        return pressedButton;
    }

    public void SetFocus(int change)
    {
        if (Enabled)
        {
            if (change == -1)
            {
                CurrentFocus = CurrentFocus + 1 == _numberOfButtons ? (byte)0 : (byte)(CurrentFocus + 1);
            }
            else if (change == 1)
            {
                CurrentFocus = CurrentFocus - 1 == -1 ? (byte)(_numberOfButtons - 1) : (byte)(CurrentFocus - 1);
            }
            for (var i = 0; i < _numberOfButtons; i++)
            {
                _buttons[i].Focus(false);
                if (CurrentFocus == i)
                {
                    _buttons[i].Focus(true);
                }
            }
        }
    }

    public void DisplayButtons()
    {
        foreach (var butt in _buttons)
        {
            butt.Display();
        }
    }
}