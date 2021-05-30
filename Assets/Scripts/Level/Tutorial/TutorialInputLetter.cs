using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialInputLetter : InputLetter
{
    // We don't want inputs to do anything on a tutorial version
    public override void OnPointerDown(PointerEventData pointerEventData){
    }

    public override void OnPointerEnter(PointerEventData pointerEventData){
    }
}
