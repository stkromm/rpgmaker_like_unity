#region

using UnityEngine;

#endregion

public class RandomTargetSelection : ITargetSelection
{
    public byte SelectTarget(BattleCombatant[] targetGroup)
    {
        var y = Random.Range(0, targetGroup.Length);
        return (byte) y;
    }
}