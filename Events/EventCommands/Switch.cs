#region

using System;

#endregion

[Serializable]
public class Switch : EventCommand
{
    public BoolInABox Lever;
    public int SkipCommands;

    public override bool Condition()
    {
        return true;
    }

    public override int OnSuccess(int numberOfCommands)
    {
        return Lever.Lever ? numberOfCommands + SkipCommands : numberOfCommands;
    }

    public override void OnGraphic()
    {
    }

    public override void OnUpdate()
    {
    }
}