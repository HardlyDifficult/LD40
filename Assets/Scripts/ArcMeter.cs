using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcMeter : MonoBehaviour
{
  Slider slider;

  public float currentValue {
    get
    {
      return slider.value;
    }
    set
    {
      slider.value = value;
    }
  }

  protected void Awake()
  {
    slider = GetComponent<Slider>();
  }

  public void UpdateArc()
  {
    float percentYPosition = Input.mousePosition.y / Screen.height;
    currentValue = percentYPosition;
  }
}
