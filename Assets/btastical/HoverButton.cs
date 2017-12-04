﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private UnityEvent clickEvent;
    
    [SerializeField]
    private Color foreColor = Color.gray;

    [SerializeField]
    private Color hoverColor = Color.white;

    private Text buttonText;

    protected void Awake()
    {
        this.buttonText = this.GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Changing color to hoverColor");
        Debug.Log(this.hoverColor);

        this.buttonText.color = this.hoverColor; // this doesnt work
        this.buttonText.color = Color.white; // this works
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Changing color to foreColor");
        Debug.Log(this.foreColor);
        this.buttonText.color = this.foreColor; // this doesnt work
        this.buttonText.color = Color.gray; // this works
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.clickEvent?.Invoke();
    }
}
