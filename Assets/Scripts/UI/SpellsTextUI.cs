using UnityEngine;
using System;
using UnityEngine.UI;

public class SpellsTextUI : MonoBehaviour
{
    Text textMain;
    Text textDetails;

    [SerializeField]
    Color[] colorPlayers = new Color[] { Color.blue, Color.red };

    [SerializeField]
    public int spellPlayerId = -1; // -1 = local player, -2 = remote player

    protected void Awake()
    {
        textMain = transform.GetChild(0).GetComponent<Text>();
        textDetails = transform.GetChild(1).GetComponent<Text>();
    }

    protected void Start()
    {
        textMain.gameObject.SetActive(false);
        textDetails.gameObject.SetActive(false);
        PlayerSpellsManager.instance.onSpellChange += OnSpellChange;

        NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
        networkController.onGameBegin += NetworkController_onGameBegin;
    }

    protected void OnDestroy()
    {
        NetworkController networkController = GameObject.FindObjectOfType<NetworkController>();
        if (networkController != null)
        {
            networkController.onGameBegin -= NetworkController_onGameBegin;
        }
    }

    void NetworkController_onGameBegin()
    {
        textMain.gameObject.SetActive(true);
        textDetails.gameObject.SetActive(true);
        Refresh();
    }

    private void OnSpellChange(int pi, int si, bool status, bool isLocalPlayer)
    {
        Refresh();
    }

    void Refresh()
    {
        int playerId = spellPlayerId >= 0 ? spellPlayerId :
            spellPlayerId == -1 ? PlayerSpellsManager.instance.LocalPlayerId :
            PlayerSpellsManager.instance.RemotePlayerId;
        bool isLocal = playerId == PlayerSpellsManager.instance.LocalPlayerId;
        if (colorPlayers != null && colorPlayers.Length > 1)
        {
            Color c = colorPlayers[playerId];
            textMain.color = c;
            textDetails.color = c;
        }
        int spellCount = 0;
        string spells = "";
        for (int i = 0; i < PlayerSpellsManager.instance.SpellCount; ++i)
        {
            if (PlayerSpellsManager.instance.GetPlayerSpellStatus(playerId, i))
            {
                spellCount++;
                if (spells.Length > 0) spells += ", ";
                spells += GameManager.instance.spells[i].name;
            }
        }
        string main;
        if (spellCount == 0)
        {
            main = isLocal ? "Your mind is all powerful" : "Your opponent is still unhindered";
        }
        else
        {
            main = isLocal ? $"You are under {spellCount} spell" : $"Your opponent is under {spellCount} spell";
            if (spellCount > 1) main += "s";
        }
        textMain.text = main;
        textDetails.text = spells;
    }
}
