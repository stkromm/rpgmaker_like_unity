#region

using System;

#endregion

[Serializable]
public class Buff
{
    public Buff(int duration, string name)
    {
        Duration = duration;
        Name = name;
    }

    public int Duration;
    public int[] Stats = new int[9];
    public int[] Resistance = new int[6];
    public string Name;

    public override string ToString()
    {
        return Duration + "\n";
    }
}