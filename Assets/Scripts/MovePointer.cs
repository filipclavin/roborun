using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class MovePointer : MonoBehaviour
{
    [SerializeField] private GameObject selected;
    [SerializeField] private ButtonClickedEvent buttonEvent;

    [SerializeField] private List<Button> buttons = new List<Button>();
    
    private void Update()
    {
        if (buttons.ElementAt(0))
        {
            selected.SetActive(true);
        }
        else
        {
            selected.SetActive(false);
        }
    }
}
