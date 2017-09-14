#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class Teleport : EventCommand
{
    public int Index;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public override void OnGraphic()
    {
    }

    public override void OnUpdate()
    {
    }

    public override int OnSuccess(int numberOfCommands)
    {
        Application.LoadLevel(Index);
        return base.OnSuccess(numberOfCommands);
    }
}