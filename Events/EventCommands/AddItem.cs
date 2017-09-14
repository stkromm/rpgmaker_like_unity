#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class AddItem : EventCommand
{
    public Vector2[] Item;
    public BoolInABox Done;
    public string Message;
    public string MessageOnReceived;
    public Announcer Ann;
    public Transform Acteur;

    public override void OnGraphic()
    {
        if (Acteur != null && Message != "")
        {
            var msg = !Done.Lever ? Message : MessageOnReceived;
            var x = Camera.main.WorldToScreenPoint(Acteur.transform.position);
            GUI.Box(new Rect(x.x, x.y, (float) Screen.width/2, (float) Screen.height/12), msg);
        }
    }

    public override bool Condition()
    {
        if (Message != "")
        {
            return Input.GetButtonDown("circuit") || Input.GetButtonDown("cross");
        }
        return true;
    }

    public override int OnSuccess(int numberOfCommands)
    {
        if (!Done.Lever)
        {
            var o = GameObject.Find("Inventory");
            var inv = o.GetComponent<Inventory>();
            for (var i = 0; i < Item.Length; i++)
            {
                inv.AddAmountofItem((int) Item[i].x, (int) Item[i].y);
                if (Ann != null)
                    Ann.AddAnnouncement(ItemDatabase.GetItemName((int) Item[i].x) + " erhalten");
            }
            Done.Lever = true;
        }
        return numberOfCommands;
    }

    public override void OnUpdate()
    {
    }
}