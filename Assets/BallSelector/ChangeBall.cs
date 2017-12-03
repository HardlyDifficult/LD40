using System;
using UnityEngine;

public class ChangeBall : MonoBehaviour
{
  GameObject parchment;


  bool isActive;
  
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
    Debug.Log("change to : " + changeTo);

    GameManager.instance.currentBallIndex = changeTo;
  }
}
