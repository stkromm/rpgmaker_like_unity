#region

using UnityEngine;

#endregion

internal enum State
{
    Main,
    Load
}

public class GameStart : MyMonoBehaviour
{
    public Texture Title;
    private State _state = State.Main;

    private void OnGUI()
    {
        switch (_state)
        {
            case State.Main:
                GUI.backgroundColor = Color.white;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Title);
                if (GUI.Button(new Rect(Screen.width / 3f, Screen.height / 3f, Screen.width / 3f, Screen.height / 20f),
                    "<size=20>New Game</size>"))
                {
                    Application.LoadLevel(0);
                }
                if (
                    GUI.Button(
                        new Rect(Screen.width / 3f, Screen.height / 3 + Screen.height / 10, Screen.width / 3f, Screen.height / 20f),
                        "<size=20>Load</size>"))
                {
                    _state = State.Load;
                }
                if (
                    GUI.Button(
                        new Rect(Screen.width / 3f, Screen.height / 3 + Screen.height / 5, Screen.width / 3f, Screen.height / 20f),
                        "<size=20>Exit</size>"))
                    Application.Quit();
                break;

            case State.Load:
                if (
                    GUI.Button(
                        new Rect(Screen.width / 3f, Screen.height / 3 + Screen.height / 10, Screen.width / 3f, Screen.height / 20f),
                        "<size=20>Save State 1</size>"))
                    // SaveStateManager.LoadGameState(1);
                    if (
                        GUI.Button(
                            new Rect(Screen.width / 3f, Screen.height / 3 + Screen.height / 5, Screen.width / 3f,
                                Screen.height / 20f), "<size=20>Back</size>"))
                        _state = State.Main;
                break;
        }
    }
}