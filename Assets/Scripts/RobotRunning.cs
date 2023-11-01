using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotRunning : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Image imageToFollow;
    private RectTransform rectImage;
    private RectTransform rectTransform;

    private void Start()
    { 
        rectImage = imageToFollow.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTimer.goingOn)
        {
            float width = rectImage.rect.width;
            Vector3 tempV = rectTransform.anchoredPosition;
            tempV.x = -width / 2;
            tempV.x += width * imageToFollow.fillAmount;
            rectTransform.anchoredPosition = tempV;
        }
    }
}
