#region

using System.Collections;
using UnityEngine;

#endregion

public class BattleActionItem : BattleActionAd
{
    public BattleActionItem(BattleCombatant attacker, short skillId, BattleCombatant defender)
        : base(attacker, skillId, defender)
    {
    }

    public override bool Valid()
    {
        var valid = true;
        var attacker = Attacker;
        if (!attacker.BattleState.CanAttack())
        {
            return false;
        }
        var o = GameObject.Find("Inventory");
        var inv = o.GetComponent<Inventory>();
        /*
         * If you would chose an item, that has an amount of 1, their is atm the possibily to
         * choose the same item again before the first item action was started and that way use an item that doesnt exist!
         * Solution atm: the second item wont be used and the action is wasted.
         * */
        if (!inv.ContainsItem(SkillId))
        {
            valid = false;
        }
        return valid;
    }

    public override IEnumerator DoAction()
    {
        IsWorking = true;
        var o = GameObject.Find("Inventory");
        var inv = o.GetComponent<Inventory>();
        if (inv.ContainsItem(SkillId))
        {
            inv.AddAmountofItem(SkillId, -1);
            Defender.DealDamage(-ItemDatabase.GetHeal(SkillId));
            Defender.DrainMana(-ItemDatabase.GetRefresh(SkillId));
            Defender.BuffParticipant(ItemDatabase.GetItemBuff(SkillId));
            var item = (UsableItem) ItemDatabase.Items()[SkillId];
            Defender.ChangeCondition(item.Condition);
            Attacker.RefreshBuffs(1);
            Attacker.BattleState.RemoveAp(Attacker.BattleState.ActionPoints);
            Action();
        }
        IsWorking = false;
        yield return 0;
    }

    public override string ToString()
    {
        return Attacker.Name + " uses " + ItemDatabase.GetItemName(SkillId);
    }
}