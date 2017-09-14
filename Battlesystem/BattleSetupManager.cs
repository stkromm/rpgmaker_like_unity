#region

using UnityEngine;

#endregion

public class BattleSetupManager : MonoBehaviour
{
    private GameObject _controller;
    private GameObject _menu;
    private GameObject _sound;
    private GameObject _modells;

    private void SetUpSound()
    {
        _sound = new GameObject { name = "sound" };
        _sound.AddComponent<AudioSource>();
        _sound.AddComponent<AudioListener>();
        _sound.AddComponent<BattleAudioManager>();
    }

    private void Start()
    {
        _controller = new GameObject { name = "BattleEngine" };
        _controller.AddComponent<BattleController>();
        _controller.AddComponent<BattleEngine>();
        _menu = new GameObject { name = "menu" };
        var modell = _menu.AddComponent<BattleMenuModell>();
        var view = _menu.AddComponent<BattleMenuView>();
        view.Modell = modell;
        var mcontroller = _menu.AddComponent<BattleMenuController>();
        mcontroller.Modell = modell;
        _modells = new GameObject { name = "modells" };
        _modells.AddComponent<BattleModellManager>();

        SetUpSound();
    }
}