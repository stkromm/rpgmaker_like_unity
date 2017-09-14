public class SkillDatabase
{
    private static SkillListSkill[] _instance;

    public static SkillListSkill[] Skills()
    {
        return _instance ?? (_instance = XmlManager.LoadSKillDatabase().Skill);
    }
}