using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatingElipse : MonoBehaviour
{
  Text text;

  string initialText;

  [SerializeField]
  float timeBetweenDots = .5f;

  int numberOfDots = 0;

  protected void Start()
  {
    text = GetComponent<Text>();
    initialText = text.text;

    StartCoroutine(AnimateElipse());
  }

  protected void OnEnable()
  {
    numberOfDots = 0;
  }

  IEnumerator AnimateElipse()
  {
    while (true)
    {
      yield return new WaitForSeconds(timeBetweenDots);
      numberOfDots++;
      if (numberOfDots > 3)
      {
        numberOfDots = 0;
      }
      text.text = initialText + new string('.', numberOfDots);
    }
  }
}
