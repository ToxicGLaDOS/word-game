using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    WordPanelHandler wordPanelHandler;
    public InputPanelLayout inputPanelLayout;
    public InputPanel inputPanel;
    public Text regexTextbox;
    public Text foundBonusWords;
    public Text messageText;
    public Text successText;
    public Button nextLevelButton;
    private LevelPresenter levelPresenter;
    // Start is called before the first frame update
    void Start()
    {
        levelPresenter = FindObjectOfType<LevelPresenter>(); 
        wordPanelHandler = FindObjectOfType<WordPanelHandler>();
    }

    public void SetRegexText(string regex){
        regexTextbox.text = regex;
    }

    public void SetInputLetters(List<char> letters){
        inputPanelLayout.Initalize(letters);
    }

    public void ScrambleHandler(){
        levelPresenter.Scramble();
    }

    public void ScrambleInput(){
        inputPanelLayout.Scramble();
    }

    public void SubmitWord(){
        levelPresenter.SubmitWord();
    }

    public string GetWord(){
        return inputPanel.InputWord;
    }

    public void InitalizeWords(List<string> words){
        wordPanelHandler.InitalizeWords(words);
    }

    public void AddCorrectWord(string word){
        wordPanelHandler.RevealWord(word);
    }

    public void AddBonusWord(string word){
        foundBonusWords.text += word + "\n";
    }

    public void DisplayMessage(string message){
        messageText.GetComponent<FadeText>().ResetAlpha();
        messageText.text = message;
    }

    public void GotoNextLevel(){
        levelPresenter.GotoNextLevel();
        successText.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
    }

    public void EndLevel(){
        successText.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);
    }

    public void AnimateUI(){
        wordPanelHandler.GetComponent<ScaleInTween>().Tween();
        inputPanelLayout.GetComponent<ScaleInTween>().Tween();
    }

}
