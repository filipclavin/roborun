using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlightY : MonoBehaviour
{
    [SerializeField] private float scaleIncrease;
    public GameObject pointer;
    public List<Button> buttons;
    public float xOffset = -50f; 
    public float yOffset = 0f;
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
                // Move the pointer if the highlighted button changes
                selected.transform.localScale *= scaleIncrease;
                MovePointerToButton(selectedButton);
                AudioManager.Instance.Play("UI_Move");
            }
        }
    }

    void MovePointerToButton(Button button)
    {
        currentButton = button;
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        
        Vector3[] worldCorners = new Vector3[4];
        buttonRect.GetWorldCorners(worldCorners);
        
        Vector3 pointerPosition = new Vector3(worldCorners[0].x + xOffset, worldCorners[0].y + yOffset, worldCorners[0].z);

        pointer.transform.position = pointerPosition;
    }


}