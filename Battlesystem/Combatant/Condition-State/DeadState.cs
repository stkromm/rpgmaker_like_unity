public class DeadState : CombatantBattleState
{
    public DeadState(CombatantBattleState a)
        : this(a.Combatant)
    {
    }

    public DeadState(BattleCombatant p)
    {
        Combatant = p;
    }

    public override void AddTickAp()
    {
        ActionPoints = 0;
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
    }

    public override bool CanMove()
    {
        return false;
    }

    public override bool CanAttack()
    {
        return false;
    }

    public override bool CanUseItems()
    {
        return false;
    }

    public override bool IsDefeated()
    {
        return true;
    }

    public override Condition GetCondition()
    {
        return Condition.Dead;
    }
}