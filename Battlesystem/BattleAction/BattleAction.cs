#region

using System.Collections;

#endregion

public abstract class BattleAction
{
    protected DoWhenFinished Action;
    public bool IsWorking;

    public delegate void DoWhenFinished();

    public virtual IEnumerator DoAction()
    {
        Action();
        yield return 0;
    }

    public void SetFinishedMethod(DoWhenFinished x)
    {
        Action = x;
    }

    public abstract bool Valid();
}