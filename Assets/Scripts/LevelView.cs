using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    public Text lettersTextbox;
    public Text regexTextbox;
    public Text foundCorrectWords;
    public Text foundBonusWords;
    public Text messageText;
    public Text wordsRemainingCounter;
    public InputField inputField;
    private LevelPresenter levelPresenter;
    // Start is called before the first frame update
    void Start()
    {
        levelPresenter = FindObjectOfType<LevelPresenter>(); 
    }

    public void SetWordsRemaining(int amount){
        wordsRemainingCounter.text = string.Format("Words remaining: {0}", amount.ToString());
    }
    public void SetLettersText(string letters){
        lettersTextbox.text = letters;
    }

    public void SetRegexText(string regex){
        regexTextbox.text = regex;
    }

    public void ScrambleHandler(){
        levelPresenter.Scramble();
    }

    public void InputHandler(){
        levelPresenter.SubmitWord();
    }

    public string GetWord(){
        return inputField.text;
    }

    public void AddCorrectWord(string word){
        foundCorrectWords.text += word + "\n";
    }

    public void AddBonusWord(string word){
        foundBonusWords.text += word + "\n";
    }

    public void DisplayMessage(string message){
        messageText.GetComponent<FadeText>().ResetAlpha();
        messageText.text = message;
    }
}
