using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public SharedData sharedData;

  public string name;

  public GameObject currentBallPrefab
  {
    get
    {
      return sharedData.ballPrefabList[currentBallIndex].gameObject;
    }
  }
  public int currentBallIndex;
    
  public SpellInfo[] spells
  {
    get
    {
      return sharedData.spellList;
    }
  }

  protected void Awake()
  {
    if(instance != null)
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);
    instance = this;
  }
}
