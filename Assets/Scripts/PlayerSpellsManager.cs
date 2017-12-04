using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellsManager : MonoBehaviour {

    public static PlayerSpellsManager instance
    {
        get; private set;
    }

    // (int playerId, int spellId, bool status, bool isLocalPlayer);
    public event Action<int, int, bool, bool> onSpellChange;

    const int PlayerCount = 2; // TODO: config
    public int SpellCount { get; private set; }
    public int LocalPlayerId { get; private set; }
    public int RemotePlayerId { get { return 1 - LocalPlayerId; } }

    public SpellState[] player1SpellState;
    public SpellState[] player2SpellState;

    public bool GetPlayerSpellStatus(int playerId, int spellId)
    {
        if (playerId == -1) playerId = LocalPlayerId;
        if (playerId == -2) playerId = RemotePlayerId;
        SpellState[] playerSpellState = (playerId == 0) ? player1SpellState : player2SpellState;
        return playerSpellState[spellId].enabled;
    }

    public void SetPlayerSpellStatus(int playerId, int spellId, bool status)
    {
        if (playerId == -1) playerId = LocalPlayerId;
        SpellState[] playerSpellState = (playerId == 0) ? player1SpellState : player2SpellState;
        playerSpellState[spellId].enabled = status;
    }

    NetworkController networkController;

    protected void Awake()
    {
        Debug.Assert(instance == null);

        instance = this;

        networkController = GameObject.FindObjectOfType<NetworkController>();
        networkController.onGameBegin += OnGameBegin;
    }

    protected void OnDestroy()
    {
        Debug.Assert(instance == this);
        if (networkController)
            networkController.onGameBegin -= OnGameBegin;
        instance = null;
    }

    protected void Start () {
        SpellInfo[] spells = GameManager.instance.spells;
        SpellCount = spells.Length;
        for (int pi = 0; pi < PlayerCount; ++pi)
        {
            SpellState[] playerSpellState = new SpellState[SpellCount];
            if (pi == 0) player1SpellState = playerSpellState;
            else         player2SpellState = playerSpellState;
            for (int si = 0; si < SpellCount; ++si)
            {
                SpellInfo info = spells[si];
                SpellState state = playerSpellState[si] = new SpellState();
                state.name = info.name;
                int copy_pi = pi; // required for lambda capture
                int copy_si = si; // required for lambda capture
                state.onSpellChange += (bool v) => notifySpellChange(copy_pi, copy_si, v);
            }
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        for (int pi = 0; pi < PlayerCount; ++pi)
        {
            SpellState[] playerSpellState = (pi == 0) ? player1SpellState : player2SpellState;
            for (int si = 0; si < SpellCount; ++si)
            {
                SpellState state = playerSpellState[si];
                if (state.enabled != state.editorEnabled)
                {
                    state.enabled = state.editorEnabled;
                }
            }
        }
#endif
    }
    
    void OnGameBegin()
    {
        LocalPlayerId = PhotonNetwork.isMasterClient ? 0 : 1;
    }

    void notifySpellChange(int playerId, int spellId, bool status)
    {
        bool isLocalPlayer = playerId == LocalPlayerId;
        print($"onSpellChange({playerId}, {spellId}, {status}, {isLocalPlayer})");
        onSpellChange?.Invoke(playerId, spellId, status, isLocalPlayer);
    }

}
