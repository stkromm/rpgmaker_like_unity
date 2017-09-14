#region

using UnityEngine;

#endregion

public class Fps : MyMonoBehaviour
{
    private float _timeA;
    public int ActualFps;
    public int LastFps;
    // Use this for initialization
    private void Start()
    {
        _timeA = Time.timeSinceLevelLoad;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(Time.timeSinceLevelLoad+" "+_timeA);
        if (Time.timeSinceLevelLoad - _timeA <= 1)
        {
            ActualFps++;
        }
        else
        {
            LastFps = ActualFps + 1;
            _timeA = Time.timeSinceLevelLoad;
            ActualFps = 0;
        }
    }

    private void OnGui()
    {
        GUI.Label(new Rect(0, 0, 30, 30), "" + LastFps);
    }
}