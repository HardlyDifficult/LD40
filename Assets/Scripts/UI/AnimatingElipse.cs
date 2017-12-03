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

  protected void Start()
  {
    text = GetComponent<Text>();
    initialText = text.text;

    StartCoroutine(AnimateElipse());
  }

  IEnumerator AnimateElipse()
  {
    int numberOfDots = 0;
    while(true)
    {
      yield return new WaitForSeconds(timeBetweenDots);
      numberOfDots++;
      if(numberOfDots > 3)
      {
        numberOfDots = 0;
      }
      text.text = initialText + new string('.', numberOfDots);
    }
  }
}
