#region

using UnityEngine;

#endregion

public class FlyInGuiElementAnimation : GuiElementAnimation
{
    private readonly float _duration;
    private readonly float _direction;

    public FlyInGuiElementAnimation(Rect rect, Color color, float direction, float duration)
        : base(rect, color)
    {
        _direction = direction;
        _duration = duration;
    }

    public override Rect GetRect()
    {
        if (Time.realtimeSinceStartup - StartTime > _duration)
        {
            AnimationOn = false;
        }
        if (AnimationOn)
        {
            var shiftedRec = Rect;
            if (_direction <= 1)
            {
                var factorX = _direction == 0f ? (Time.realtimeSinceStartup - StartTime) / _duration : 1;
                var factorY = _direction == 1f ? (Time.realtimeSinceStartup - StartTime) / _duration : 1;
                shiftedRec.Set(Rect.xMin * factorX, Rect.yMin * factorY, Rect.width, Rect.height);
            }
            else
            {
                var factorX = _direction == 2f ? (Time.realtimeSinceStartup - StartTime) / _duration : 1;
                var factorY = _direction == 3f ? (Time.realtimeSinceStartup - StartTime) / _duration : 1;
                shiftedRec.Set(Screen.width - (Screen.width - Rect.xMin) * factorX,
                    Screen.height - (Screen.height - Rect.yMin) * factorY, Rect.width, Rect.height);
            }
            return shiftedRec;
        }
        return Rect;
    }

    public override Color GetColor()
    {
        if (Time.realtimeSinceStartup - StartTime > _duration)
        {
            AnimationOn = false;
        }
        return AnimationOn ? Color.Lerp(Color.yellow, Color, (Time.realtimeSinceStartup - StartTime) / _duration) : Color;
    }
}