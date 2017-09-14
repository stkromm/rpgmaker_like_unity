#region

using System;
using System.Collections;
using UnityEngine;

#endregion

public class BattleActionSkill : BattleActionAd
{
    public BattleActionSkill(BattleCombatant attacker, short skillId, BattleCombatant defender)
        : base(attacker, skillId, defender)
    {
    }

    public override bool Valid()
    {
        var valid = true;
        var attacker = Attacker;
        if (!attacker.BattleState.CanAttack())
        {
            Debug.Log("x");
            return false;
        }
        if (!SkillDatabase.Skills().ValidIndex(SkillId))
        {
            Debug.Log("y");
            return false;
        }
        var skill = SkillDatabase.Skills()[SkillId];
        if (attacker.Mana < skill.Cost)
        {
            Debug.Log("z");
            valid = false;
        }
        switch ((AttackRange)skill.Range)
        {
            case AttackRange.Area:
                break;

            case AttackRange.Side:
                break;

            case AttackRange.Single:
                break;
        }

        return valid;
    }

    public override IEnumerator DoAction()
    {
        var q = GameObject.Find("sound").GetComponent<AudioSource>();

        IsWorking = true;
        var o = GameObject.Find("modells").GetComponent<BattleModellManager>();
        var a = GameObject.Find("BattleEngine").GetComponent<BattleController>();
        var x =
            o.ParticipantsModells.ToArray()[Array.IndexOf(a.Participants.ToArray(), Attacker)]
                .GetComponentInChildren<BattleAnimation>();
        yield return new WaitForSeconds(x.ChangeState(BattleAnimationType.Attack));
        yield return new WaitForSeconds(0.5f);
        var skill = SkillDatabase.Skills()[SkillId];
        //if (!attacker.DrainMana(skill.Cost)) return;
        var damage = skill.Damage + Attacker.Attack - Defender.Defense;
        Defender.DealDamage(damage);
        var Clip = Resources.Load<AudioClip>("sounds/battle/damage");
        q.PlayOneShot(Clip);
        o = GameObject.Find("modells").GetComponent<BattleModellManager>();
        a = GameObject.Find("BattleEngine").GetComponent<BattleController>();
        x =
            o.ParticipantsModells.ToArray()[Array.IndexOf(a.Participants.ToArray(), Defender)]
                .GetComponentInChildren<BattleAnimation>();
        if (Defender.BattleState.GetType() == typeof(DeadState))
        {
            yield return new WaitForSeconds(x.ChangeState(BattleAnimationType.Dead));
        }
        else
        {
            yield return new WaitForSeconds(x.ChangeState(BattleAnimationType.Hurt));
        }
        Attacker.RefreshBuffs(1);
        Action();
        Attacker.BattleState.RemoveAp(skill.Actioncost);
        IsWorking = false;
        yield return 0;
        /*
                if (Defender.battleState.IsDefeated())
                {
                    var o = new GameObject();
                    if (o.FindSafe("BattleEngine").GetComponent<BattleController>().ActiveHero == Defender) { o.FindSafe("BattleEngine").GetComponent<BattleController>().RemoveActiveHero(); }
                }
          */
    }

    public override string ToString()
    {
        return Attacker.Name + " does " + SkillDatabase.Skills()[SkillId].Name;
    }
}