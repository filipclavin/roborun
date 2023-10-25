using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class MainButton : Selectable
{
    [SerializeField] private GameObject selected;
    [SerializeField] private ButtonClickedEvent buttonEvent;
    public ButtonClickedEvent onClick
    {
        get { return buttonEvent; }
        set { buttonEvent = value; }
    }
    
    private void Update()
    {
        if (IsHighlighted())
        {
            selected.SetActive(true);
        }
        else
        {
            selected.SetActive(false);
        }

        if (IsPressed())
        {
            onClick.Invoke();
        }
    }
}
