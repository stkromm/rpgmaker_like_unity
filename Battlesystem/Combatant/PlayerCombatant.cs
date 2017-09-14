public class PlayerCombatant : BattleCombatant
{
    public Character Ch;

    public PlayerCombatant(Character ch)
    {
        Health = (int)ch.Attributes[0].GetActualValue(ch.Level);
        Mana = (int)ch.Attributes[1].GetActualValue(ch.Level);
        Attack = (int)ch.Attributes[2].GetActualValue(ch.Level);
        Name = ch.CharacterName;
        Ch = ch;
        BattleState = new NormalState(this);
        ChangeCondition((Condition)ch.Condition);
    }

    #region BattleCombatant implemented Methods

    public override void DealDamage(int damage)
    {
        Ch.Attributes[0].TakeDamage(damage, Ch.Level);
        Health = (int)Ch.Attributes[0].GetActualValue(Ch.Level);
        if (Health > 0) return;
        BattleState.ChangeCondition(Condition.Dead);
        Ch.DealCondition((int)BattleState.GetCondition());
    }

    public override bool DrainMana(int mana)
    {
        var x = Ch.Attributes[1].TakeDamage(mana, Ch.Level);
        Mana = (int)Ch.Attributes[1].GetActualValue(Ch.Level);
        return x;
    }

    public override bool BuffParticipant(Buff b)
    {
        var x = Ch.BuffCharacter(b);
        Health = (int)Ch.Attributes[0].GetActualValue(Ch.Level);
        Mana = (int)Ch.Attributes[1].GetActualValue(Ch.Level);
        Attack = (int)Ch.Attributes[2].GetActualValue(Ch.Level);
        BattleState.ChangeCondition((Condition)Ch.Condition);
        return x;
    }

    public override void RefreshBuffs(int i)
    {
        Ch.RefreshBuffCounter(i);
        Health = (int)Ch.Attributes[0].GetActualValue(Ch.Level);
        Mana = (int)Ch.Attributes[1].GetActualValue(Ch.Level);
        Attack = (int)Ch.Attributes[2].GetActualValue(Ch.Level);
        BattleState.ChangeCondition((Condition)Ch.Condition);
    }

    public override void ChangeCondition(Condition c)
    {
        Ch.DealCondition((int)c);
        base.ChangeCondition(c);
        BattleState.ChangeCondition((Condition)Ch.Condition);
    }

    #endregion BattleCombatant implemented Methods
}