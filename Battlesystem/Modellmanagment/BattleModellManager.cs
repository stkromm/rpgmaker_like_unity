#region

using System.Collections.Generic;
using UnityEngine;

#endregion

public class BattleModellManager : MonoBehaviour
{
    public List<GameObject> ParticipantsModells = new List<GameObject>();

    private void Start()
    {
        var p = GameObject.Find("Party").GetSafeComponent<Party>();
        for (var i = 0; i < p.GroupLength(); i++)
        {
            if (p.GetCharacterInParty(i).InParty && p.GetCharacterInParty(i).InCombatParty)
            {
                // Load Modell
                var modell = Resources.Load<GameObject>("battle/modells/noModell");
                modell = (GameObject) Instantiate(modell);
                // Do positioning
                modell.transform.position = new Vector3(67 - i*2, 1, 37 + i*p.GroupParty.Length*3);
                var q = Quaternion.Euler(modell.transform.rotation.x, modell.transform.rotation.y + 180f,
                    modell.transform.rotation.z);
                modell.transform.rotation = q;
                ParticipantsModells.Add(modell);
            }
        }
        var a = Resources.Load<GameObject>("battle/Enemies");
        a = (GameObject) Instantiate(a);
        var f = Resources.Load<GameObject>("battle/Enemygroups");
        f = (GameObject) Instantiate(f);

        var index = 0;
        if (PlayerPrefs.HasKey("monstergroup"))
        {
            index = PlayerPrefs.GetInt("monstergroup");
        }
        for (var i = 0; i < f.GetComponent<EnemyPartyDatabase>().Database.Array[index].Array.Length; i++)
        {
            var modell = Resources.Load<GameObject>("battle/modells/Troll");
            modell = (GameObject) Instantiate(modell);

            modell.transform.position = new Vector3(37 + i*2, 1,
                37 + 3*f.GetComponent<EnemyPartyDatabase>().Database.Array[index].Array.Length*i);
            ParticipantsModells.Add(modell);
        }
    }
}