public abstract class BattleActionAd : BattleAction
{
    public BattleCombatant Attacker { private set; get; }

    public short SkillId { private set; get; }

    public BattleCombatant Defender { private set; get; }

    protected BattleActionAd(BattleCombatant attacker, short skillId, BattleCombatant defender)
    {
        Attacker = attacker;
        Defender = defender;
        SkillId = skillId;
    }

    public override bool Valid()
    {
        return true;
    }
}