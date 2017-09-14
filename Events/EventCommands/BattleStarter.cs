#region

using UnityEngine;

#endregion

public class BattleStarter : EventCommand
{
    public int Monstergroup;

    public override int OnSuccess(int numberOfCommands)
    {
        PlayerPrefs.SetInt("monstergroup", Monstergroup);
        var o = GameObject.Find("Player");
        PlayerPrefs.SetFloat("playerpositionx", o.transform.position.x);
        PlayerPrefs.SetFloat("playerpositiony", o.transform.position.y);
        PlayerPrefs.SetFloat("playerpositionz", o.transform.position.z);
        AutoFade.LoadLevel(1, 0.2f, 1.5f, Color.black);

        return base.OnSuccess(numberOfCommands);
    }

    public override void OnGraphic()
    {
    }

    public override void OnUpdate()
    {
    }
}