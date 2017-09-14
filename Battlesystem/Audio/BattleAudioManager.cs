#region

using UnityEngine;

#endregion

public class BattleAudioManager : MonoBehaviour
{
    private BattleController _controller;
    private BattleMode _mode = BattleMode.Started;
    private AudioSource _player;

    private void Start()
    {
        _controller = GameObject.Find("BattleEngine").GetComponent<BattleController>();
        _player = GetComponentInParent<AudioSource>();
        _player.enabled = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_mode == _controller.Mode) return;
        _mode = _controller.Mode;
        switch (_mode)
        {
            case BattleMode.Installed:
                var music =
                    Resources.Load<AudioClip>(PlayerPrefs.HasKey("battlemusic")
                        ? PlayerPrefs.GetString("battlemusic")
                        : "sounds/battle/battle1");
                _player.loop = true;
                _player.volume = 40;
                _player.clip = music;
                _player.Play();
                break;

            case BattleMode.Started:
                break;

            case BattleMode.Won:
                var victory =
                    Resources.Load<AudioClip>(PlayerPrefs.HasKey("battlevictory")
                        ? PlayerPrefs.GetString("battlevictory")
                        : "sounds/battle/victory1");
                _player.Stop();
                _player.loop = false;
                _player.clip = victory;
                _player.Play();
                break;

            case BattleMode.Lose:
                break;
        }
    }
}