using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelPresenter : MonoBehaviour
{
    private Level level;
    private LevelView levelView;

    private int seed = 2;

    // Start is called before the first frame update
    void Start()
    {
        levelView = FindObjectOfType<LevelView>();
        level = new Level(GlobalValues.dictionaryPath);
        Initalize();

    }

    void Initalize(){
        level.InitalizeFromLevel(GlobalValues.levelData);
        seed++;
        List<char> letters = level.Letters;
        string regex = level.RegexDefinition;
        
        string regexText = string.Format("Regex: {0}", regex);

        SetLettersText(letters);
        levelView.SetRegexText(regexText);
        levelView.SetInputLetters(letters);
        levelView.InitalizeWords(level.GetRemainingWords());

    }

    public void SubmitWord(){
        string word = levelView.GetWord();
        Level.WordValidity validity = level.GetValidity(word);

        switch(validity){
            case Level.WordValidity.Found:
                levelView.DisplayMessage("You already found that word.");
                break;
            case Level.WordValidity.Correct:
                CorrectWord(word);
                break;
            case Level.WordValidity.Bonus:
                level.AddFoundWord(word);
                levelView.AddBonusWord(word);
                levelView.DisplayMessage("You found a bonus word!");
                break;
            case Level.WordValidity.Real:
                levelView.DisplayMessage("You can't use some of those characters!");
                break;
            case Level.WordValidity.Fake:
                levelView.DisplayMessage("That's not a real word.");
                break;
            default:
                throw new System.Exception(string.Format("Found unimplemented validity {0}", validity.ToString()));
        }
    }

    void CorrectWord(string word){
        level.AddFoundWord(word);
        int wordsRemaining = level.GetRemainingWords().Count;
        levelView.AddCorrectWord(word);
        levelView.DisplayMessage("You found a correct word!");

        // If we found all the words
        if (level.FoundAllWords()){
            levelView.EndLevel();
        }
    }

    public void GotoNextLevel(){
        int nextLevelIndex = int.Parse(GlobalValues.levelData.name) + 1;
        LevelData nextLevelData = Resources.Load<LevelData>(string.Format("LevelData/{0}/{1}", GlobalValues.levelData.group, nextLevelIndex.ToString()));
        if(nextLevelData != null){
            GlobalValues.levelData = nextLevelData;
            Initalize();
        }
        else{
            // TODO: Either go to next group or generate level from here
            Debug.Log("Out of levels.");
        }
    }

    public void Solve(){
        foreach(string word in level.correctWords){
            CorrectWord(word);
        }
    }

    public void Scramble(){
        level.ScrambleLetters();
        SetLettersText(level.Letters);
        levelView.ScrambleInput();
    }

    void SetLettersText(List<char> letters){
        string lettersText = string.Format("Letters: {0}", new string(letters.ToArray()));
    }
}
