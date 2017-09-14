#region

using UnityEngine;

#endregion

public class GameSetup : MyMonoBehaviour
{
    public GameObject Inventory;
    public GameObject Party;

    public void Start()
    {
        if (GameObject.Find("Party") == null)
            Party = (GameObject)Instantiate(Party);
        if (GameObject.Find("Inventory") == null)
            Inventory = (GameObject)Instantiate(Inventory);
        Party.name = "Party";
        Inventory.name = "Inventory";
        if (!PlayerPrefs.HasKey("playerpositionx")) return;
        var o = GameObject.Find("Player");
        if (o == null) return;
        // o.transform.position = new Vector3(PlayerPrefs.GetFloat("playerpositionx"), PlayerPrefs.GetFloat("playerpositiony"), PlayerPrefs.GetFloat("playerpositionz"));
    }
}