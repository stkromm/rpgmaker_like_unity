using System.Collections;
using UnityEngine;

public class AnimationSoundEffect : MonoBehaviour
{
    public AudioClip Clip;

    public void PlaySoundEffect(float value)
    {
        var o = GameObject.Find("sound").GetComponent<AudioSource>();
        o.PlayOneShot(Clip);
    }
}