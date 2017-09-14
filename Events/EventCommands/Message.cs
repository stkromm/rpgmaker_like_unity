#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class Message : EventCommand
{
    public Transform Acteur;
    public string MessageString;

    public override void OnGraphic()
    {
        var x = Camera.main.WorldToScreenPoint(Acteur.transform.position);
        GUI.Box(new Rect(x.x, x.y, (float) Screen.width/2, (float) Screen.height/12), MessageString);
    }

    public override bool Condition()
    {
        return Input.GetButtonDown("circuit") || Input.GetButtonDown("cross");
    }

    public override int OnSuccess(int numberOfCommands)
    {
        return numberOfCommands;
    }

    public override void OnUpdate()
    {
    }
}