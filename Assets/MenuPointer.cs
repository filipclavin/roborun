using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class MenuPointer : MonoBehaviour
{
    public GameObject point;
    private int selectedButton = 1;

    public List<Transform> buttonPositions;
    void Start()
    {
        
    }
    private void OnButtonUp()
    {
        // Checks if the pointer needs to move down or up, in this case the poiter moves up one button
        if (buttonPositions.Count - 1 < 1)
        {
            selectedButton -= 1;
        }
        MoveThePointer();
        return;
    }
    private void OnButtonDown()
    {
        // Checks if the pointer needs to move down or up, in this case the poiter moves down one button
        if (selectedButton < buttonPositions.Count - 1)
        {
            selectedButton += 1;
        }
        MoveThePointer();
        Debug.Log("weeeeeeeee");
        return;
    }
    private void MoveThePointer()
    {
        point.transform.position = buttonPositions.ElementAt(selectedButton).position;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //OnButtonDown();
    }
}
