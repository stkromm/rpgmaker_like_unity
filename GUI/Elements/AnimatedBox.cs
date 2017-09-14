#region

using UnityEngine;

#endregion

public class AnimatedBox
{
    private Rect _rec;
    public string Content;
    public bool Animated;
    public bool AnimatedAtStart;
    private readonly GuiElementAnimation _anim;

    public AnimatedBox(string content, bool animated, bool start, GuiElementAnimation anim)
    {
        Content = content;
        Animated = animated;
        AnimatedAtStart = start;
        _anim = anim;
        if (start)
        {
            anim.StartAnimation();
        }
    }

    public void Display()
    {
        GUI.skin.box.normal.textColor = _anim.GetColor();
        GUI.Box(_anim.GetRect(), Content);
    }
}