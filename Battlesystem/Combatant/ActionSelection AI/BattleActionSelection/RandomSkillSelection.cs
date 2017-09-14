#region

using UnityEngine;

#endregion

namespace Assets.Scripts.Battlesystem.Combatant.BattleActionSelection
{
    internal class RandomSkillSelection : IBattleActionSelection
    {
        public Vector2 SelectAction(BattleCombatant acteur, int[] skills)
        {
            var z = Random.Range(0, skills.Length);
            return new Vector2(z, 0);
        }
    }
}