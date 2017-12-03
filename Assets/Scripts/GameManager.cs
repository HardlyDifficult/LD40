using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  public SharedData sharedData;

  public GameObject currentBallPrefab
  {
    get
    {
      return sharedData.ballPrefabList[currentBallIndex].gameObject;
    }
  }
  public int currentBallIndex;

  protected void Awake()
  {
    if(instance != null)
    {
      Destroy(gameObject);
    }

    DontDestroyOnLoad(gameObject);
    instance = this;
  }
}
