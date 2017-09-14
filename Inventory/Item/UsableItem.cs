public class UsableItem : Item
{
    public int Heal;
    public int Refresh;
    public Condition Condition;

    public override string ToString()
    {
        var s = "";
        if (Description != "")
            s += "'" + "<i>" + Description + "</i>'\n\n";
        if (Heal != 0)
            s += "Heals   \t " + Heal + "\n";
        if (Refresh != 0)
            s += "Refreshs   \t" + Refresh + "\n";
        if (Condition != 0)
            s += "Cures " + Condition + " Status\n";
        if (BuffId != 0)
            s += "Buffs with " + BuffDatabase.Buffs()[BuffId].Name + "\n";
        if (Unique)
        {
            s += "<color=yellow><b>Unique</b></color>\n\n";
        }
        if (Value != 0)
            s += "\n<color=grey>Sell-Value</color>\t" + Value;
        return s;
    }
}