#region

using UnityEngine;

#endregion

public class CameraSwitchCheck : MonoBehaviour
{
    public CameraSkipCondition[] SkipConditions;
    // Use this for initialization

    // Update is called once per frame
    private void Update()
    {
        foreach (var c in SkipConditions)
        {
            c.Skip(GetComponent<Camera>());
        }
    }
}