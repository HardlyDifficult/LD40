using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPlayNow : MonoBehaviour
{
  public void Play()
  {
    SceneManager.LoadScene("PrototypeScene");
  }
}
