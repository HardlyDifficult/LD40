using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCloudio : MonoBehaviour
{
    [SerializeField]
    public int spellPlayerId = -1; // -1 = local player
    [SerializeField]
    public int spellId = 0;
    [SerializeField]
    public bool spellEnabled;
    [SerializeField]
    public float spellTransitionTime = 5;
    [SerializeField]
    private string shaderFactorName = "_FogHeight";
    [SerializeField]
    private float shaderFactorMin = -0.5f;
    [SerializeField]
    private float shaderFactorMax = 1.0f;

    public float spellFactor = 0.0f;
    public MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;
        PlayerSpellsManager.instance.onSpellChange += OnSpellChange;
    }

    protected void OnDestroy()
    {
        if (PlayerSpellsManager.instance)
            PlayerSpellsManager.instance.onSpellChange -= OnSpellChange;
    }

    private void OnSpellChange(int pi, int si, bool status, bool isLocalPlayer)
    {
        if (si == spellId && (pi == spellPlayerId || (isLocalPlayer && spellPlayerId == -1)))
        {
            spellEnabled = status;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!spellEnabled) return;
        renderer.enabled = true;
        if (spellFactor < 1.0)
        {
            spellFactor += Time.deltaTime / spellTransitionTime;
            renderer.material.SetFloat(shaderFactorName, shaderFactorMin + (shaderFactorMax- shaderFactorMin) * spellFactor);
        }
    }
}
