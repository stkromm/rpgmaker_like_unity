#region

using System;
using System.Linq;
using Assets.Scripts.Battlesystem.Combatant.BattleActionSelection;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

[Serializable]
public class EnemyCombatant : BattleCombatant, ICloneable
{
    public int[] SkillList;
    public int BehaviourSkill;
    public int BehaviourTarget;
    private IBattleActionSelection _skillSelector;
    private ITargetSelection _targetSelector;

    public void Initiliaze()
    {
        BattleState = new NormalState(this);
        switch (BehaviourSkill)
        {
            case 0:
                _skillSelector = new RandomSkillSelection();
                break;
        }
        switch (BehaviourSkill)
        {
            case 0:
                _targetSelector = new RandomTargetSelection();
                break;
        }
    }

    private BattleAction DoSomething()
    {
        var o = new GameObject();
        var x = o.FindSafe("BattleEngine").GetSafeComponent<BattleController>();
        Object.Destroy(o);
        var y = _targetSelector.SelectTarget(x.AlliedTeam.ToArray());
        return new BattleActionSkill(this, (short) _skillSelector.SelectAction(this, SkillList).x,
            x.AlliedTeam.ToArray()[y]);
    }

    #region BattleCombatant implemented Methods

    public override void BattleLoopCall()
    {
        if (BattleState.ActionPoints != ActionPointsCap || DidAction) return;
        var o = new GameObject();
        SetBattleAction(o.FindSafe("BattleEngine").GetSafeComponent<BattleController>(), DoSomething());
        Object.Destroy(o);
    }

    public override void DealDamage(int damage)
    {
        Health -= damage;
        if (Health > 0) return;
        Health = 0;
        BattleState.ChangeCondition(Condition.Dead);
    }

    public override bool DrainMana(int mana)
    {
        if (Mana - mana < 0) return false;
        Mana -= mana;
        return true;
    }

    public override bool BuffParticipant(Buff b)
    {
        return true;
    }

    public override void RefreshBuffs(int i)
    {
    }

    #endregion BattleCombatant implemented Methods

    public object Clone()
    {
        return MemberwiseClone();
    }
}