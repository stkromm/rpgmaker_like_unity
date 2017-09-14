#region

using System;
using System.Collections;
using UnityEngine;

#endregion

public enum BattleAnimationType
{
    Idle = 0,
    Attack = 2,
    Special,
    Magic,
    Victory,
    Dead = 3,
    Use,
    Hurt = 1
}

public class BattleAnimation : MonoBehaviour
{
    public BattleAnimationType Typ;
    public Animator Animator;

    public float ChangeState(BattleAnimationType typ)
    {
        if (Typ == BattleAnimationType.Dead)
        {
            return 0f;
        }
        Typ = typ;
        switch (typ)
        {
            case BattleAnimationType.Hurt:
                Animator.SetInteger("state", 1);
                break;

            case BattleAnimationType.Attack:
                Animator.SetInteger("state", 2);
                break;

            case BattleAnimationType.Dead:
                Animator.SetInteger("state", 3);
                break;

            default:
                Animator.SetInteger("state", 0);
                break;
        }
        AnimatorClipInfo[] animationClips = Animator.GetCurrentAnimatorClipInfo(0);
        if (animationClips.Length == 0)
        {
            throw new Exception("No clips associated with animation");
        }

        var animationClip = animationClips[0].clip;
        var animationTime = animationClip.averageDuration;
        Debug.Log("Clip with length" + animationTime);
        StartCoroutine(Reset(1.5f));
        return 1.5f;
    }

    public IEnumerator Reset(float time)
    {
        yield return new WaitForSeconds(time);
        if (Typ != BattleAnimationType.Dead)
        {
            Animator.SetInteger("state", 0);
        }
        yield return 0;
    }
}