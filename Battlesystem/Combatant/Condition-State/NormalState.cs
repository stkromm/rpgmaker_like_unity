#region

using UnityEngine;

#endregion

public class NormalState : CombatantBattleState
{
    public NormalState(CombatantBattleState a)
        : this(a.Combatant)
    {
    }

    public NormalState(BattleCombatant p)
    {
        Combatant = p;
        if (p.BattleState != null)
        {
            ActionPoints = p.BattleState.ActionPoints;
        }
        else
        {
            ActionPoints = 950;
        }
    }

    public override void AddTickAp()
    {
        float x = Random.Range(1, 3);
        var addAp = (int) Mathf.Ceil((x*(1 + Combatant.Speed/25f)));
        ActionPoints = ActionPoints + addAp > BattleCombatant.ActionPointsCap
            ? BattleCombatant.ActionPointsCap
            : ActionPoints + addAp;
    }

    public override void ChangeCondition(Condition c)
    {
        switch (c)
        {
            case Condition.Blind:
                Combatant.BattleState = new BlindState(Combatant);
                break;

            case Condition.Cold:
                Combatant.BattleState = new ColdState(Combatant);
                break;

            case Condition.Cursed:
                Combatant.BattleState = new CursedState(Combatant);
                break;

            case Condition.Dead:
                Combatant.BattleState = new DeadState(Combatant);
                break;

            case Condition.Heat:
                Combatant.BattleState = new HeatState(Combatant);
                break;

            case Condition.Paralysed:
                Combatant.BattleState = new ParalysedState(Combatant);
                break;

            case Condition.Poision:
                Combatant.BattleState = new PoisionState(Combatant);
                break;

            case Condition.Stunned:
                Combatant.BattleState = new StunnedState(Combatant);
                break;

            case Condition.Wounded:
                Combatant.BattleState = new WoundedState(Combatant);
                break;
        }
    }

    public override void RemoveAp(int amount)
    {
        ActionPoints -= amount;
    }

    public override bool CanMove()
    {
        return true;
    }

    public override bool CanAttack()
    {
        return true;
    }

    public override bool CanUseItems()
    {
        return true;
    }

    public override bool IsDefeated()
    {
        return true;
    }

    public override Condition GetCondition()
    {
        return Condition.Normal;
    }
}