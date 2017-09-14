#define DEBUG

#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#endregion

public enum BattleMode
{
    Installed,
    Started,
    Won,
    Lose
}

public class BattleController : MyMonoBehaviour
{
    #region Attributes

    // TO-DO Audio-Manager
    //
    private bool _actionBarStop;

    private float _actionPointStartTime;
    public const float BattleSpeed = 0.025f;
    private BattleEngine _engine;

    public BattleMode Mode { private set; get; }

    public List<BattleCombatant> Participants { private set; get; }

    public PlayerCombatant ActiveHero { private set; get; }

    public IEnumerable<BattleCombatant> AlliedTeam
    {
        get
        {
            return Participants == null
                ? null
                : Participants.Where(p => p is PlayerCombatant).ToArray();
        }
    }

    public IEnumerable<BattleCombatant> EnemyTeam
    {
        get
        {
            return Participants == null
                ? null
                : Participants.Where(p => p.GetType() == typeof(EnemyCombatant)).ToArray();
        }
    }

    #endregion Attributes

    #region SETUP METHODS

    private void SetUpData()
    {
        Participants = new List<BattleCombatant>();
        ActiveHero = null;
        var o = new GameObject();
        o.FindSafe("Inventory").GetSafeComponent<MenuController>().enabled = false;
        var p = o.FindSafe("Party").GetSafeComponent<Party>();
        Destroy(o);
        for (var i = 0; i < p.GroupLength(); i++)
        {
            if (p.GetCharacterInParty(i).InParty && p.GetCharacterInParty(i).InCombatParty)
            {
                var pq = new PlayerCombatant(p.GetCharacterInParty(i));
                Participants.Add(pq);
            }
        }
        var enemyDatabase = Resources.Load<GameObject>("battle/Enemies");
        enemyDatabase = (GameObject)Instantiate(enemyDatabase);
        var enemyGroupDatabase = Resources.Load<GameObject>("battle/Enemygroups");
        enemyGroupDatabase = (GameObject)Instantiate(enemyGroupDatabase);

        var count = 0;
        var index = 0;
        if (PlayerPrefs.HasKey("monstergroup"))
        {
            index = PlayerPrefs.GetInt("monstergroup");
        }
        for (var i = 0;
            i < enemyGroupDatabase.GetComponent<EnemyPartyDatabase>().Database.Array[index].Array.Length;
            i++)
        {
            var par =
                (EnemyCombatant)
                    enemyDatabase.GetComponent<EnemyDatabase>().Database[
                        enemyGroupDatabase.GetComponent<EnemyPartyDatabase>().Database.Array[index].Array[i]].Clone();

            Participants.Add(par);
            par.Initiliaze();
            count++;
        }
    }

    private void Start()
    {
        _engine = GetComponentInParent<BattleEngine>();
        SetUpData();
        Mode = BattleMode.Installed;
    }

    #endregion SETUP METHODS

    private void Update()
    {
        if (Participants == null) return;
        if (Mode != BattleMode.Installed)
        {
            return;
        }
        if (!_actionBarStop && Time.realtimeSinceStartup - _actionPointStartTime > BattleSpeed)
        {
            FillActionBars();
            _actionPointStartTime = Time.realtimeSinceStartup;
        }
        UpdateActiveHero();

        #region Battle-Loop: Let every combatant use an action if possible, check if the battle is done and, if not, dequeue a new action from the queue

        foreach (var e in Participants)
        {
            // Combatants, which the player doesn't controll, can enqueue a new action, if they are ready.
            e.BattleLoopCall();
        }
        if (!CheckEnd())
        {
            _engine.Dequeue();
        }

        #endregion Battle-Loop: Let every combatant use an action if possible, check if the battle is done and, if not, dequeue a new action from the queue
    }

    public bool AddBattleAction(BattleAction x)
    {
        return _engine.AddBattleAction(x);
    }

    private void UpdateActiveHero()
    {
        if (ActiveHero != null)
        {
            if (ActiveHero.DidAction || ActiveHero.BattleState.ActionPoints != BattleCombatant.ActionPointsCap)
            {
                ActiveHero = null;
            }
        }
        if (ActiveHero != null) return;
        foreach (var p in AlliedTeam)
        {
            ActiveHero = p.BattleState.ActionPoints == 1000 && !p.DidAction
                ? (PlayerCombatant)p
                : ActiveHero;
        }
    }

    private void FillActionBars()
    {
        foreach (var p in Participants)
        {
            p.AddActionPoints();
        }
    }

    private bool CheckEnd()
    {
        if (EnemyTeam == null) return true;
        var x = EnemyTeam.Aggregate(0, (current, p) => p.Health > 0 ? current + 1 : current);
        var y = AlliedTeam.Aggregate(0, (current, p) => p.Health > 0 ? current + 1 : current);
        if (x != 0 && y != 0)
        {
            return false;
        }
        if (y == 0)
        {
            Mode = BattleMode.Lose;
        }
        else if (x == 0)
        {
            Mode = BattleMode.Won;
        }
        EndBattle();
        return true;
    }

    private void EndBattle()
    {
        var o = new GameObject();
        o.FindSafe("Inventory").GetSafeComponent<MenuController>().enabled = true;
        _actionBarStop = true;
    }
}