using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonReturnToMenu : MonoBehaviour
{
  public void GoToMenu()
  {
    SceneManager.LoadScene("Menu");
  }
}
