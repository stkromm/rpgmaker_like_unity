#region

using System.Collections;
using UnityEngine;

#endregion

public class AutoFade : MonoBehaviour
{
    private static AutoFade _mInstance;
    private Material _mMaterial;
    private string _mLevelName = "";
    private int _mLevelIndex;
    private bool _mFading;

    private static AutoFade Instance
    {
        get { return _mInstance ?? (_mInstance = (new GameObject("AutoFade")).AddComponent<AutoFade>()); }
    }

    public static bool Fading
    {
        get { return Instance._mFading; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _mInstance = this;
        _mMaterial =
            new Material(
                "Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
    }

    private void DrawQuad(Color aColor, float aAlpha)
    {
        aColor.a = aAlpha;
        _mMaterial.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(aColor); // moved here, needs to be inside begin/end
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        var t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, t);
        }
        if (_mLevelName != "")
            Application.LoadLevel(_mLevelName);
        else
            Application.LoadLevel(_mLevelIndex);
        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
            DrawQuad(aColor, t);
        }
        _mFading = false;
    }

    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        _mFading = true;
        StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
    }

    public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance._mLevelName = aLevelName;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }

    public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        if (Fading) return;
        Instance._mLevelName = "";
        Instance._mLevelIndex = aLevelIndex;
        Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
    }
}