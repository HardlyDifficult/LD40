using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLoadScene: MonoBehaviour
{
  [SerializeField]
  string scene;

  public void GoToScene()
  {
    SceneManager.LoadScene(scene);
  }
}
