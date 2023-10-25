using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonSelect : MonoBehaviour
{
    public MainButton primaryButton;
    void Start()
    {
        primaryButton.Select();
    }
    

    public void Button()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        primaryButton.Select();
    }

    
}