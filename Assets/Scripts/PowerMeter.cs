using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{
  bool isChargeIncreasing;

  [SerializeField]
  float speed = 1;

  Slider slider;

  public float currentValue
  {
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

  public void Charge()
  {
    float currentValue = this.currentValue;

    float delta = speed * Time.deltaTime;
    if (isChargeIncreasing == false)
    {
      delta *= -1;
    }

    currentValue += delta;
    print($"{currentValue} with delta {delta}");

    if (currentValue < 0
      || currentValue > 1)
    {
      isChargeIncreasing = !isChargeIncreasing;
      if (currentValue < 0)
      {
        currentValue = -currentValue;
      }
      else
      {
        currentValue = 1 + currentValue;
      }
    }

    this.currentValue = currentValue;
  }

  public void Reset()
  {
    currentValue = 0;
    isChargeIncreasing = true;
  }
}
