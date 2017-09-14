#region

using UnityEngine;

#endregion

public static class GameObjectExtension
{
    public static T GetSafeComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        var component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError("Expected to find component of type "
                           + typeof (T) + " but found none", obj);
        }

        return component;
    }

    public static GameObject FindSafe(this GameObject obj, string s)
    {
        var objec = GameObject.Find(s);
        if (objec == null)
        {
            Debug.LogError("<color=red>FATAL ERROR:</color> Couldn't find GameObject " + s +
                           ". Nullpointer Exception possible!");
        }
        return objec;
    }
}