#region

using UnityEngine;

#endregion

public class ColorFlashGuiElementAnimation : GuiElementAnimation
{
    private readonly float _duration;
    private readonly Color _color2;
    private readonly bool _loop;

    public ColorFlashGuiElementAnimation(Rect rect, Color color1, Color color2, float duration, bool loop)
        : base(rect, color1)
    {
        _loop = loop;
        _color2 = color2;
        _duration = duration;
    }

    public override Rect GetRect()
    {
        if (Time.realtimeSinceStartup - StartTime > _duration)
        {
            if (_loop)
            {
                StartTime = Time.realtimeSinceStartup;
            }
            else
            {
                AnimationOn = false;
            }
        }
        if (AnimationOn)
        {
            return Rect;
        }
        return Rect;
    }

    public override Color GetColor()
    {
        if (Time.realtimeSinceStartup - StartTime > _duration)
        {
            if (_loop)
            {
                StartTime = Time.realtimeSinceStartup;
            }
            else
            {
                AnimationOn = false;
            }
        }
        if (AnimationOn)
        {
            return Color.Lerp(Color, _color2, (Time.realtimeSinceStartup - StartTime) / _duration);
        }
        return Color;
    }
}