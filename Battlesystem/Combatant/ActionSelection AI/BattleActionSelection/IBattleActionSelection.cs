#region

using UnityEngine;

#endregion

public interface IBattleActionSelection
{
    Vector2 SelectAction(BattleCombatant acteur, int[] skills);
}