using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterHandler : MonoBehaviour
{
    public Text letterText;
    // Start is called before the first frame update
    void Start()
    {
        letterText = GetComponentInChildren<Text>();
        letterText.gameObject.SetActive(false);
    }

    public void Reveal(){
        letterText.gameObject.SetActive(true);
    }
}
