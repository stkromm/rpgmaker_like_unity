public class EquipableItem : Item
{
    public int[] Stats = new int[9];
    public ArmorSlot Slot;

    public override string ToString()
    {
        var lang = new Language();
        var s = "";
        if (Description != "")
            s += "'" + "<i>" + Description + "</i>'\n\n";

        for (var i = 0; i < Stats.Length; i++)
        {
            if (i < lang.PropItems.Length)
                s += lang.PropItems[i] + "\t" + Stats[i] + "\n";
        }
        s += lang.PropItems[10] + "\t" + Slot + "\n";
        if (Unique)
        {
            s += "<color=yellow><b>Unique</b></color>\n\n";
        }
        if (Value != 0)
            s += "\n<color=grey>Sell-Value</color>\t" + Value;
        return s;
    }
}