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


  protected void Awake()
  {
    text = GetComponent<Text>();
    initialText = text.text;
  }

  protected void OnEnable()
  {
    StartCoroutine(AnimateElipse());
  }

  protected void OnDisable()
  {
    StopAllCoroutines();
  }

  IEnumerator AnimateElipse()
  {
    text.text = initialText;
    int numberOfDots = 0;

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
