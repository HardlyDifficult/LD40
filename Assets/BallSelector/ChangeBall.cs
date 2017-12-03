using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBall : MonoBehaviour
{
  public static ChangeBall instance;

  GameObject parchment;

  public event Action onBallPreferenceChange;

  public GameObject currentBallPrefab
  {
    get
    {
      return ballPrefabList[currentBallIndex].gameObject;
    }
  }

  bool isActive;

  /// <summary>
  /// These must be in a Resources directory.
  /// </summary>
  [SerializeField]
  GameObject[] ballPrefabList = null;

  int currentBallIndex;

  protected void Awake()
  {
    Debug.Assert(instance == null);
    Debug.Assert(ballPrefabList.Length > 0, $"{nameof(ChangeBall)} needs to be configured");

    instance = this;
  }

  protected void OnDestroy()
  {
    Debug.Assert(instance == this);

    instance = null;
  }

  void Start()
  {
    //Debug.Log("list count : " + ListOfBalls.Count);

    ChangeTheBall(0); //activate the standard ball

    parchment = gameObject.transform.Find("Parchment").gameObject;
    isActive = parchment.activeSelf;
    if (isActive == true)
    {
      //this will deactivate the UI at the start of the game
      OpenClosePanel();
    }
  }

  public void OpenClosePanel()
  {
    isActive = !isActive;
    parchment.SetActive(isActive);
  }

  public void ChangeTheBall(
    int changeTo)
  {
    Debug.Assert(changeTo < ballPrefabList.Length);
    Debug.Log("change to : " + changeTo);

    currentBallIndex = changeTo;
    onBallPreferenceChange?.Invoke();
  }
}
