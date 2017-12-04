using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SharedData", menuName = "HD/ScriptableObjects/SharedData", order = 1)]
public class SharedData : ScriptableObject
{
  /// <summary>
  /// These must be in a Resources directory.
  /// </summary>
  [SerializeField]
  public GameObject[] ballPrefabList = null;

  [SerializeField]
  public SpellInfo[] spellList = null;
}
