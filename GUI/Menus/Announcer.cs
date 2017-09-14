#region

using System.Collections;
using UnityEngine;

#endregion

public class Announcer : MyMonoBehaviour
{
    public AudioClip Sound;
    public GUISkin Skin;
    private readonly Queue _announcements = new Queue();
    private bool _announcing;
    private string _message = "";
    private float _timeToWait = 3.0f;
    private float _timeStarted;
    private bool _announcingDelay = true;

    // Update is called once per frame
    private void Update()
    {
        _timeToWait = _announcements.Count == 0 ? 1.5f : 1.5f / _announcements.Count / 3;
        if (!_announcingDelay)
        {
            _announcingDelay = true;
        }
        if (!_announcing && _announcements.Count != 0)
        {
            _message = "";
            var s = "";
            var o = _announcements.Dequeue();
            if (o is string)
            {
                s = o.ToString();
            }
            if (s == "") return;
            _message = s;
            GetComponent<AudioSource>().PlayOneShot(Sound);
            _timeStarted = Time.realtimeSinceStartup;
            _announcing = true;
            return;
        }
        _announcing = !(Time.realtimeSinceStartup - _timeStarted >= _timeToWait);
    }

    private void OnGUI()
    {
        GUI.skin = Skin;
        if (_announcing && _message != "")
            GUI.Box(new Rect(Screen.width / 2f - Screen.width / 10f, 0, Screen.width / 10f, Screen.height / 12f), _message);
    }

    public void AddAnnouncement(string s)
    {
        if (_announcingDelay)
        {
            _announcements.Enqueue(s);
        }
    }
}