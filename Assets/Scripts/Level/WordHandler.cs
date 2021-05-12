using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHandler : MonoBehaviour
{
    List<LetterHandler> letters = new List<LetterHandler>();
    public void Initalize(){
        foreach (Transform child in transform){
            letters.Add(child.GetComponent<LetterHandler>());
        }
    }

    public void Reveal(){
        foreach (LetterHandler letter in letters){
            letter.Reveal();
        }
    }
}
