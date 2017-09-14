#region

using System;
using System.Linq;
using UnityEngine;

#endregion

public class BattleMenuController : MyMonoBehaviour
{
    public GameObject Cursor;
    public BattleMenuModell Modell;

    [HideInInspector]
    public bool TargettingTeam;

    #region MonoBehaviour

    private void Update()
    {
        Modell.MainMenu.Enabled = Modell.Engine.ActiveHero != null;
        PressedTriangleButton();
        PressedSquaredButton();
        PressedCircuitButton();
        PressedCrossButton();
        // Order of the following calls is important. A input otherwise goes through all menus
        if (Modell.TargetMenu.Enabled)
        {
            Modell.TargetMenu.CheckInput(TargetMenuButtonPressed, Sound);
        }
        else if (Modell.ChooseMenu.Enabled)
        {
            Modell.ChooseMenu.CheckInput(ChooseMenuButtonPressed, Sound);
            if (Input.GetButtonDown("l1"))
            {
                Modell.ItemListIndex++;
                Modell.UpdateSelectionState();
            }
            if (Input.GetButtonDown("r1"))
            {
                Modell.ItemListIndex--;
                Modell.UpdateSelectionState();
            }
        }
        else
        {
            Modell.MainMenu.CheckInput(MainMenuButtonPressed, Sound);
        }
    }

    #endregion MonoBehaviour

    #region Input Check Methods

    private void ChooseMenuButtonPressed()
    {
        Modell.Skill = Modell.Items.ToArray()[Modell.ChooseMenu.CurrentFocus].Id;
        switch (Modell.Selection)
        {
            case SelectionState.SkillSelection:
                Modell.SetupTargetSetup(Modell.Engine.EnemyTeam.ToArray());
                TargettingTeam = false;
                break;

            case SelectionState.ItemSelection:
                Modell.SetupTargetSetup(Modell.Engine.AlliedTeam.ToArray());
                TargettingTeam = true;
                break;
        }
        Modell.ChangeSelectionState(SelectionState.TargetSelection);
    }

    private void MainMenuButtonPressed()
    {
        if (Modell.MainMenu.CurrentFocus == 0)
        {
            Modell.SetupTargetSetup(Modell.Engine.EnemyTeam.ToArray());
            TargettingTeam = false;
            Modell.Skill = 0;
            Modell.ChangeSelectionState(SelectionState.TargetSelection);
        }
        else
        {
            Modell.ChangeSelectionState((SelectionState)Modell.MainMenu.CurrentFocus);
        }
    }

    private void PressedCircuitButton()
    {
        if (!Input.GetButtonDown("circuit")) return;
        switch (Modell.Selection)
        {
            case SelectionState.TargetSelection:
                Modell.ChangeSelectionState(Modell.LastSelectedMenu);
                break;

            default:
                Modell.ChangeSelectionState(SelectionState.ActionMenu);
                break;
        }
    }

    private void PressedCrossButton()
    {
        if (!Input.GetButtonDown("cross")) return;
        if (Modell.Engine.Mode == BattleMode.Won)
        {
            AutoFade.LoadLevel(0, 0.2f, 1.5f, Color.black);
        }
    }

    private void PressedSquaredButton()
    {
        if (!Modell.TargetMenu.Enabled || !Input.GetButtonDown("square")) return;
        TargettingTeam = !TargettingTeam;
        Modell.SetupTargetSetup(TargettingTeam ? Modell.Engine.AlliedTeam.ToArray() : Modell.Engine.EnemyTeam.ToArray());
    }

    private void PressedTriangleButton()
    {
        if (Input.GetButtonDown("triangle"))
        {
        }
    }

    private void TargetMenuButtonPressed()
    {
        Modell.Target = Modell.TargetMenu.CurrentFocus;
        if (!TargettingTeam)
        {
            Modell.Target += (byte)Modell.Engine.AlliedTeam.ToArray().Length;
        }
        if (((Modell.LastSelectedMenu == SelectionState.SkillSelection ||
              Modell.LastSelectedMenu == SelectionState.ActionMenu) &&
             Modell.Engine.ActiveHero.SetBattleAction(Modell.Engine,
                 new BattleActionSkill(Modell.Engine.ActiveHero, Modell.Skill, Modell.Engine.Participants[Modell.Target]))) ||
            (Modell.LastSelectedMenu == SelectionState.ItemSelection &&
             Modell.Engine.ActiveHero.SetBattleAction(Modell.Engine,
                 new BattleActionSkill(Modell.Engine.ActiveHero, Modell.Skill, Modell.Engine.Participants[Modell.Target]))))
        {
            Modell.ChangeSelectionState(SelectionState.ActionMenu);
        }
    }

    #endregion Input Check Methods

    public void CursorAnimation()
    {
        var i = TargettingTeam
            ? Modell.TargetMenu.CurrentFocus
            : Modell.TargetMenu.CurrentFocus + Modell.Engine.AlliedTeam.ToArray().Length;
        Cursor.transform.position = Modell.ParticipantsModells.ToArray()[i].transform.position;
        Cursor.transform.position = new Vector3(Cursor.transform.position.x,
            Cursor.transform.position.y + 3 + (float)Math.Sin(Time.realtimeSinceStartup * 2) * 0.3f,
            Cursor.transform.position.z);
    }

    private void Sound()
    {
    }
}