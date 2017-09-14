public static class MyNumberUtility
{
    public static int ClampNumber(int ground, int value)
    {
        return value < ground ? ground : value;
    }

    public static int ClampNumber(int ground, int up, int value)
    {
        return ClampNumber(ground, value) >= up ? up - 1 : value;
    }
}