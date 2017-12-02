using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
  public static UIController instance
  {
    get; private set;
  }

  [SerializeField]
  public PowerMeter powerMeter = null;

  [SerializeField]
  public ArcMeter arcMeter = null;

  protected void Awake()
  {
    Debug.Assert(instance == null);

    instance = this;
  }

  protected void OnDestroy()
  {
    Debug.Assert(instance == this);

    instance = null;
  }

  public void EnableThrowUI()
  {
    powerMeter.gameObject.SetActive(true);
    arcMeter.gameObject.SetActive(true);
  }

  public void DisableThrowUI()
  {
    powerMeter.gameObject.SetActive(false);
    arcMeter.gameObject.SetActive(false);
  }
}
