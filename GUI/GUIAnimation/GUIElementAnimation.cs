#region

using UnityEngine;

#endregion

public class GuiElementAnimation
{
    public bool AnimationOn;
    protected float StartTime;
    protected Color Color;
    protected Rect Rect;

    public GuiElementAnimation(Rect rect, Color color)
    {
        Color = color;
        Rect = rect;
    }

    public void StartAnimation()
    {
        AnimationOn = true;
        StartTime = Time.realtimeSinceStartup;
    }

    public virtual Rect GetRect()
    {
        return Rect;
    }

    public virtual Color GetColor()
    {
        return Color;
    }
}