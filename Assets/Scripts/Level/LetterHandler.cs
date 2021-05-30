using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterHandler : MonoBehaviour
{
    public Vector3 revealScaleFactor;
    [Range(0, 0.5f)]
    public float scaleTime;
    public Text letterText;
    // Start is called before the first frame update
    void Start()
    {
        letterText = GetComponentInChildren<Text>(true);
        letterText.gameObject.SetActive(false);
    }

    public void Reveal(){
        if(letterText != null){
            letterText.gameObject.SetActive(true);
            LeanTween.scale(gameObject, revealScaleFactor, scaleTime).setLoopPingPong(1);
        }
    }

    public void Hide(){
        if(letterText != null){
            letterText.gameObject.SetActive(false);
        }
    }
}
