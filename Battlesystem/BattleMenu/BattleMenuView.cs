#region

using System.Linq;
using UnityEngine;

#endregion

public class BattleMenuView : MonoBehaviour
{
    private readonly int _scHi = Screen.height;
    private readonly int _scWi = Screen.width;
    private GameObject _combatant;

    //
    public BattleMenuModell Modell;

    private void OnGUI()
    {
        switch (Modell.Engine.Mode)
        {
            case BattleMode.Installed:
                BattleMenu();
                break;

            case BattleMode.Won:
                GUI.Label(new Rect(0, 0, _scWi, _scHi), "<size=50>WON</size>");
                break;

            case BattleMode.Lose:
                GUI.Label(new Rect(0, 0, _scWi, _scHi), "<size=50>LOSE</size>");
                break;
        }
    }

    private void BattleMenu()
    {
        CharacterStatus();
        // MonsterStatus();
        if (Modell.Engine.ActiveHero == null) return;
        switch (Modell.Selection)
        {
            case SelectionState.TargetSelection:

                Modell.TargetMenu.DisplayButtons();
                break;

            default:
                ActionMenu();
                break;
        }
    }

    private void MonsterStatus()
    {
        var s = Modell.Engine.EnemyTeam.Aggregate("",
            (current, c) =>
                current +
                (c.Name + " HP:" + c.Health + " //MP:" + c.Mana + " //COND:" + c.BattleState.GetCondition() + " AP:" +
                 c.BattleState.ActionPoints + "\n"));
        GUI.Label(new Rect(0, _scHi / 15f, _scWi / 2f, _scHi / 15f), "<size=25>" + s + "</size>");
    }

    private void CharacterStatus()
    {
        for (var i = 0; i < Modell.Engine.AlliedTeam.ToArray().Length; i++)
        {
            Debug.Log("partymemebers");
            if (!Modell.CharactersHealth[i].Changing)
                StartCoroutine(Modell.CharactersHealth[i].ChangeCounter(Modell.Engine.AlliedTeam.ToArray()[i].Health));
            Modell.CharactersHealth[i].Display();
            if (!Modell.CharactersMana[i].Changing)
                StartCoroutine(Modell.CharactersMana[i].ChangeCounter(Modell.Engine.AlliedTeam.ToArray()[i].Mana));
            Modell.CharactersMana[i].Display();
            GUI.Label(new Rect(_scWi / 10f * 9f, _scHi / 20f * (16 + i), _scWi / 10f, _scHi / 20f),
                "" + Modell.Engine.AlliedTeam.ToArray()[i].BattleState.ActionPoints);
        }
    }

    private void ActionMenu()
    {
        GUI.Box(new Rect(_scWi / 3 * 2, 0, _scWi / 3f, _scHi / 12f), Modell.Engine.ActiveHero.Name);
        if (Modell.MainMenu.Enabled)
            Modell.MainMenu.DisplayButtons();
        if (Modell.Selection == SelectionState.SkillSelection)
        {
            Modell.ChooseMenu.DisplayButtons();
        }
        if (Modell.Selection == SelectionState.TacticSelection)
        {
            Modell.ChooseMenu.DisplayButtons();
        }
        if (Modell.Selection == SelectionState.ItemSelection)
        {
            Modell.ChooseMenu.DisplayButtons();
        }
    }
}