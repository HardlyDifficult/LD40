using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAfter : MonoBehaviour
{
  [SerializeField]
  float time;

  Text text;

  protected void Start()
  {
    StartCoroutine(Show());
  }

  IEnumerator Show()
  {
    text = GetComponent<Text>();
    text.enabled = false;
    yield return new WaitForSeconds(time);
    text.enabled = true;
  }
}
