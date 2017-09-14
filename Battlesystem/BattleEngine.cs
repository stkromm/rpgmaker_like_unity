#region

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class BattleEngine : MonoBehaviour
{
    #region Attributes

    private readonly Queue<BattleAction> _actionList = new Queue<BattleAction>();
    private bool _queueStop;
    private float _actionStartTime;
    public const float EnqueueDelay = 2f;
    private string _message = "";
    private GuiElementAnimation _a;
    private AnimatedBox _b;

    #endregion Attributes

    public void Dequeue()
    {
        if (_actionList.Count == 0)
        {
            return;
        }
        if (_queueStop || (Time.realtimeSinceStartup - _actionStartTime < EnqueueDelay))
        {
            return;
        }
        _actionStartTime = Time.realtimeSinceStartup;
        StopQueue();
        StartCoroutine(DoAction(_actionList.Dequeue()));
    }

    public bool AddBattleAction(BattleAction x)
    {
        if (x.Valid())
        {
            _actionList.Enqueue(x);
            return true;
        }
        return false;
    }

    private IEnumerator DoAction(BattleAction a)
    {
        _message = a.ToString();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(a.DoAction());
        while (a.IsWorking)
        {
            yield return 1;
        }
        yield return new WaitForSeconds(1);
        _message = "";
        UnstopQueue();
        yield return 0;
    }

    private void StopQueue()
    {
        _queueStop = true;
    }

    private void UnstopQueue()
    {
        _queueStop = false;
    }

    private void OnGui()
    {
        if (_message != "")
        {
            if (_a == null)
            {
                _a =
                    new ColorFlashGuiElementAnimation(
                        new Rect(Screen.width*0.30f, 0, Screen.width*0.4f, Screen.height/20), Color.red, Color.white,
                        0.5f, false);
                _b = new AnimatedBox(_message, true, true, _a);
            }
            _b.Display();
        }
        else
        {
            _a = null;
        }
    }
}