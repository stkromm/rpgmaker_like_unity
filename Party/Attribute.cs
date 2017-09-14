#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class Attribute
{
    public float AttributeCap = 100;
    public float AttributeGroundValue = 0;
    public float AdditionalArmorStats = 0;
    public float AdditionalBuffStats = 0;
    public string Name = "";
    public float Factor = 1;
    public float Damage;

    public bool TakeDamage(float a, float level)
    {
        var value = GetValueCap(level);
        if (a == 0f) return false;
        if ((int)Damage == 0 && a < 0 || (int)Damage == (int)value && a > 0)
        {
            Debug.Log("Take " + a + " Damage doesnt work");
            return false;
        }
        Damage += a;
        Damage = Damage < 0 ? 0 : Damage;
        Damage = Damage > value ? value : Damage;
        return true;
    }

    public float GetActualValue(float level)
    {
        var value = GetValueCap(level);
        value -= Damage;
        return value;
    }

    public float GetValueCap(float level)
    {
        var a = Mathf.Ceil(Mathf.Exp(level) * Factor);
        a = a > AttributeCap ? AttributeCap : a;
        return a + AdditionalArmorStats + AdditionalBuffStats;
    }
}