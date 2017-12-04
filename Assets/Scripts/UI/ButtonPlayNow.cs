using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPlayNow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  private Text buttonText;

  [SerializeField]
  Text nameText;

  public void Play()
  {
    GameManager.instance.name = nameText.text;
    SceneManager.LoadScene("PrototypeScene");
  }

  protected void Awake()
  {
    this.buttonText = this.GetComponentInChildren<Text>();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.buttonText.color = Color.white;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.buttonText.color = Color.gray;
  }
}
