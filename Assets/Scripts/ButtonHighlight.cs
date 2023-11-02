using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour
{
    [SerializeField] private float scaleIncrease;
    public GameObject pointer; // The pointer GameObject
    public List<Button> buttons; // List of buttons on the canvas
    public float xOffset = -50f; // Offset on the X-axis to position pointer on the left side of the button

    private Button currentButton;

    void Start()
    {
       
        MovePointerToButton(buttons[0]);
    }

    void Update()
    {
      
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null)
        {
            Button selectedButton = selected.GetComponent<Button>();
            if (selectedButton != null && selectedButton != currentButton)
            {
                if (currentButton != null)
                {
                    currentButton.transform.localScale /= scaleIncrease;
                }
                selected.transform.localScale *= scaleIncrease;
                // Move the pointer if the highlighted button changes
                MovePointerToButton(selectedButton);
            }
        }
    }

    void MovePointerToButton(Button button)
    {
        
        currentButton = button;
        
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        Vector3 buttonPosition = buttonRect.position;
        pointer.transform.position = new Vector3(buttonPosition.x + xOffset, buttonPosition.y, buttonPosition.z);
    }
}