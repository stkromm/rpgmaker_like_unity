#region

using System;
using UnityEngine;

#endregion

[Serializable]
public class Event : MyMonoBehaviour
{
    public EventCommand[] Commands;
    private int _commandCounter;
    private bool _show;
    private bool _activated;
    private bool _isColliding;
    private ThirdPersonController _move;

    [NonSerialized]
    private MenuController _menu;

    public GUISkin Skin;

    private void Update()
    {
        if (!_show)
        {
            return;
        }
        _move.enabled = false;
        _menu.enabled = false;
        if (!_activated)
        {
            _activated = true;
            return;
        }
        Commands[_commandCounter].OnUpdate();
        if (!Commands[_commandCounter].Condition())
        {
            return;
        }
        _commandCounter = Commands[_commandCounter].OnSuccess(_commandCounter);
        if (_commandCounter < Commands.Length - 1)
        {
            _commandCounter++;
            return;
        }
        _move.enabled = true;
        _menu.enabled = true;
        _commandCounter = 0;
        _show = false;
        _activated = false;
    }

    private void OnGUI()
    {
        if (!_show)
        {
            if (_isColliding)
            {
                GUI.skin = Skin;
                var screenCoordinates = Camera.main.WorldToScreenPoint(transform.position);
                GUI.Label(new Rect(screenCoordinates.x, screenCoordinates.y, Screen.width / 20, Screen.height / 20),
                    "<color=yellow><b>Press X</b></color>");
            }
            return;
        }
        if (!(_commandCounter >= Commands.Length))
        {
            Commands[_commandCounter].OnGraphic();
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        _move = GameObject.Find("Player").GetComponent<ThirdPersonController>();
        _menu = GameObject.Find("Inventory").GetComponent<MenuController>();
        _isColliding = true;
        _activated = false;
    }

    private void OnTriggerStay(Collider coll)
    {
        if (_show)
        {
            return;
        }
        if (Input.GetButtonDown("cross"))
        {
            _show = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        _isColliding = false;
        _show = false;
        _activated = false;
        _commandCounter = 0;
    }
}