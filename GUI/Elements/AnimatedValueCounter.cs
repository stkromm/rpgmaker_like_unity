#region

using System.Collections;
using UnityEngine;

#endregion

public class AnimatedValueCounter
{
    private int _value;
    private readonly int _maxValue;
    private readonly string _title;
    public bool Changing;
    public bool Animated;
    public bool AnimatedAtStart;
    public float FlashSpeed = 3f;
    private readonly GuiElementAnimation _anim;

    public AnimatedValueCounter(int value, int maxValue, string title, bool animated, bool start,
        GuiElementAnimation anim)
    {
        _value = value;
        _maxValue = maxValue;
        _title = title;
        Animated = animated;
        AnimatedAtStart = start;
        _anim = anim;
    }

    public IEnumerator ChangeCounter(int i)
    {
        Changing = true;
        var x = i - _value;
        while (i != _value)
        {
            if (x > 0)
            {
                _value++;
            }
            else
            {
                _value--;
            }
            yield return 3;
        }
        Changing = false;
        yield return 0;
    }

    public void Display()
    {
        GUI.skin.label.normal.textColor = _anim.GetColor();
        GUI.Label(_anim.GetRect(), "<size=30>" + _title + ":" + _value + "//" + _maxValue + "</size>");
    }
}