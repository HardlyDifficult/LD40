using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellState
{
    public String name;
    public event Action<bool> onSpellChange;
    private bool _enabled;
    public bool enabled
    {
        get { return _enabled; }
        set
        {
            if (value != _enabled)
            {
                _enabled = value;
#if UNITY_EDITOR
                editorEnabled = value;
#endif
                onSpellChange?.Invoke(value);
            }
        }
    }
#if UNITY_EDITOR
    public bool editorEnabled;
#endif
}
