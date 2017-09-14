#region

using System;

#endregion

// Pentagram shaped
public enum BattlePosition
{
    Middle,
    LeftTop,
    LeftBottom,
    RightTop,
    RightBottom
}

[Serializable]
public abstract class BattleCombatant
{
    public int Attack;
    public int Defense;
    public int Speed;
    public int Intellect;
    public int Parry;
    public int Luck;
    public int Reflect;
    public int[] Resistance;
    public int Health;
    public int Mana;

    public CombatantBattleState BattleState { set; get; }

    public BattlePosition Position { protected set; get; }

    public string Name;

    public const int ActionPointsCap = 1000;

    #region Action Handling

    public bool DidAction { protected set; get; }

    private void SetDidAction()
    {
        DidAction = false;
    }

    public bool SetBattleAction(BattleController controller, BattleAction x)
    {
        x.SetFinishedMethod(SetDidAction);
        if (!controller.AddBattleAction(x)) return false;
        DidAction = true;
        return true;
    }

    #endregion Action Handling

    public virtual void AddActionPoints()
    {
        BattleState.AddTickAp();
    }

    public virtual void ChangePosition(BattlePosition pos)
    {
        Position = pos;
    }

    public virtual void BattleLoopCall()
    {
    }

    public virtual void ChangeCondition(Condition c)
    {
        BattleState.ChangeCondition(c);
    }

    public abstract void DealDamage(int damage);

    public abstract bool DrainMana(int mana);

    public abstract bool BuffParticipant(Buff b);

    public abstract void RefreshBuffs(int i);
}