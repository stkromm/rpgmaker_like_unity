#region

using UnityEngine;

#endregion

public class CameraSkipCondition : MonoBehaviour
{
    public Camera NextCamera;

    public void Skip(Camera c)
    {
        if (CanSkip(c))
        {
            c.enabled = false;
            NextCamera.enabled = true;
        }
    }

    public int YAngleMin;
    public int YAngleMax;
    public int ZAngleMin;
    public int ZAngleMax;
    public int XAngleMin;
    public int XAngleMax;

    public bool CanSkip(Camera c)
    {
        var cameraY = c.transform.rotation.eulerAngles.y;
        var cameraX = c.transform.rotation.eulerAngles.x;
        var cameraZ = c.transform.rotation.eulerAngles.z;
        if ((cameraY > YAngleMin && cameraY < YAngleMax) && (cameraX > XAngleMin && cameraX < XAngleMax) &&
            (cameraZ > ZAngleMin && cameraZ < ZAngleMax))
        {
            return true;
        }
        return false;
    }
}