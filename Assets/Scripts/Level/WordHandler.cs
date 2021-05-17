using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHandler : MonoBehaviour
{
    [Range(0, .5f)]
    public float timeBetweenLetterReveals;
    List<LetterHandler> letters = new List<LetterHandler>();
    public void Initalize(){
        foreach (Transform child in transform){
            letters.Add(child.GetComponent<LetterHandler>());
        }
    }

    public void Reveal(){
        for(int i = 0; i < letters.Count; i++){
            LetterHandler letter = letters[i];
            LeanTween.delayedCall(timeBetweenLetterReveals * i, letter.Reveal);
        }
    }
}
