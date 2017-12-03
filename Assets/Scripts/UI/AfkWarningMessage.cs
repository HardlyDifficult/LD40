using System;
using UnityEngine;

public class AfkWarningMessage : MonoBehaviour
{
  public static GameObject instance;

  protected void Awake()
  {
    instance = gameObject;
  }

}
