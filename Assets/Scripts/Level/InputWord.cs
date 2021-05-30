using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputWord : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>(true);
    }

    public void AddLetter(char letter){
        if(text == null){
            text = GetComponentInChildren<Text>(true);
        }
        gameObject.SetActive(true);
        text.text += letter.ToString();
    }

    public void RemoveLetter(){
        text.text = text.text.Remove(text.text.Length - 1);
    }

    public void Clear(){
        text.text = "";
        gameObject.SetActive(false);
    }
}
