public abstract class CombatantBattleState
{
    protected BattleCombatant C;

    public int ActionPoints { protected set; get; }

    public BattleCombatant Combatant
    {
        get { return C; }
        set { C = value; }
    }

    public abstract void RemoveAp(int amount);

    public abstract void AddTickAp();

    public abstract void ChangeCondition(Condition c);

    public virtual bool CanMove()
    {
        return true;
    }

    public virtual bool CanAttack()
    {
        return true;
    }

    public virtual bool CanUseItems()
    {
        return true;
    }

    public virtual bool IsDefeated()
    {
        return false;
    }

    public abstract Condition GetCondition();
}