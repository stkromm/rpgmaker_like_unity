public class MiscItem : Item
{
    public override string ToString()
    {
        var s = "";
        if (Description != "")
            s += "'" + "<i>" + Description + "</i>'\n\n";
        if (Unique)
        {
            s += "<color=yellow><b>Unique</b></color>\n\n";
        }
        if (Value != 0)
            s += "\n<color=grey>Sell-Value</color>\t" + Value;
        return s;
    }
}