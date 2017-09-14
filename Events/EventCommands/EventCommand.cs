#region

using System;

#endregion

[Serializable]
public abstract class EventCommand : MyMonoBehaviour
{
    public virtual int OnSuccess(int numberOfCommands)
    {
        return numberOfCommands;
    }

    public abstract void OnGraphic();

    public virtual bool Condition()
    {
        return true;
    }

    public abstract void OnUpdate();
}