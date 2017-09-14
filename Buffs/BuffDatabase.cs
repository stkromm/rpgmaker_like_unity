public static class BuffDatabase
{
    private static Buff[] _instance;

    public static Buff[] Buffs()
    {
        return _instance ?? (_instance = XmlManager.LoadBuffDatabase());
    }
}