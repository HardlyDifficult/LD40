using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

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
