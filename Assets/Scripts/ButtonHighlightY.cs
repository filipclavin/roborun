using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlightY : MonoBehaviour
{
    public GameObject pointer; // The pointer GameObject
    public List<Button> buttons; // List of buttons on the canvas
    public float xOffset = -50f; // Offset on the X-axis to position pointer on the left side of the button
    public float yOffset = 0f; // Offset on the Y-axis to fine-tune vertical position

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
                // Move the pointer if the highlighted button changes
                MovePointerToButton(selectedButton);
            }
        }
    }

    void MovePointerToButton(Button button)
    {
        currentButton = button;
        RectTransform buttonRect = button.GetComponent<RectTransform>();

        // Get the bottom-left corner position of the button in world coordinates
        Vector3[] worldCorners = new Vector3[4];
        buttonRect.GetWorldCorners(worldCorners);

        // Position the pointer based on the bottom-left corner and apply the offsets
        Vector3 pointerPosition = new Vector3(worldCorners[0].x + xOffset, worldCorners[0].y + yOffset, worldCorners[0].z);

        pointer.transform.position = pointerPosition;
    }


}