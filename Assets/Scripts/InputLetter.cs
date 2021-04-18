using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputLetter : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    InputPanel inputPanel;
    // Start is called before the first frame update
    void Start()
    {
        inputPanel = transform.parent.GetComponent<InputPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData pointerEventData){
        inputPanel.BeginInput();
        inputPanel.LetterSelected(gameObject);
    }

    public void OnPointerEnter(PointerEventData pointerEventData){
        inputPanel.LetterSelected(gameObject);
    }
}
