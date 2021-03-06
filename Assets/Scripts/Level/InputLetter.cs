using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class InputLetter : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    InputPanel inputPanel;
    Text text;
    public Image selectedCircle;
    public char Letter {
        get{
            return char.Parse(text.text);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inputPanel = transform.parent.parent.GetComponent<InputPanel>();
        text = GetComponentInChildren<Text>(true);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void StopMoving(){
        StopAllCoroutines();
    }

    public IEnumerator MoveTo(Vector2 startingPosition, Vector2 endingPosition, float speed){
        float time = 0;
        while(time < 1){
            transform.localPosition = Vector2.Lerp(startingPosition, endingPosition, time);
            time += speed * Time.deltaTime;
            yield return null;
        }
    }

    // Called by InputPanel when a letter is selected
    public void SelectLetter(){
        selectedCircle.gameObject.SetActive(true);
    }

    public void DeselectLetter(){
        selectedCircle.gameObject.SetActive(false);
    }

    // The no argument versions of OnPointer{Enter|Down} exist so 
    // we can assign them in the editor for tutorial versions of the inputLetter.
    public void OnPointerDown(){
        inputPanel.BeginInput();
        inputPanel.LetterSelected(this);
    }

    public void OnPointerEnter(){
        inputPanel.LetterSelected(this);
    }

    virtual public void OnPointerDown(PointerEventData pointerEventData){
        OnPointerDown();
    }

    virtual public void OnPointerEnter(PointerEventData pointerEventData){
        OnPointerEnter();
    }
}
