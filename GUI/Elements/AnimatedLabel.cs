#region

using UnityEngine;

#endregion

public class AnimatedLabel
{
    private readonly Rect _rec;
    private readonly GUIContent _content;
    public bool Animated;
    public bool AnimatedAtStart;
    public float FlashSpeed = 3f;

    public AnimatedLabel(Rect rec, GUIContent content, bool animated, bool start)
    {
        _content = content;
        _rec = rec;
        Animated = animated;
        AnimatedAtStart = start;
    }

    public void Display()
    {
        GUI.skin.label.normal.textColor = Color.Lerp(Color.white, Color.magenta,
            (Mathf.Sin(Time.realtimeSinceStartup * FlashSpeed) + 1) / 2.0f);
        GUI.Label(_rec, _content);
    }
}