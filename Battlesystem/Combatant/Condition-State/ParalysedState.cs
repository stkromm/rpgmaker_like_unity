#region

using System;

#endregion

public class ParalysedState : CombatantBattleState
{
    public ParalysedState(CombatantBattleState a)
        : this(a.Combatant)
    {
    }

    public ParalysedState(BattleCombatant p)
    {
        Combatant = p;
    }

    public override void AddTickAp()
    {
    }

    public override void ChangeCondition(Condition c)
    {
        switch (c)
        {
            case Condition.Normal:
                Combatant.BattleState = new NormalState(this);
                break;
        }
    }

    public override void RemoveAp(int amount)
    {
        throw new NotImplementedException();
    }

    public override bool CanMove()
    {
        throw new NotImplementedException();
    }

    public override bool CanAttack()
    {
        throw new NotImplementedException();
    }

    public override bool CanUseItems()
    {
        throw new NotImplementedException();
    }

    public override bool IsDefeated()
    {
        throw new NotImplementedException();
    }

    public override Condition GetCondition()
    {
        throw new NotImplementedException();
    }
}