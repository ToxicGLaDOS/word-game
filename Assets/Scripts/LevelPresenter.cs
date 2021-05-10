using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPresenter : MonoBehaviour
{
    private Level level;
    private LevelView levelView;
    // Start is called before the first frame update
    void Start()
    {
        levelView = FindObjectOfType<LevelView>();
        level = new Level("Assets/Resources/words.txt");
        level.InitalizeRandom();

        List<char> letters = level.Letters;
        string regex = level.RegexDefinition;
        
        string regexText = string.Format("Regex: {0}", regex);

        SetLettersText(letters);
        levelView.SetRegexText(regexText);
        levelView.SetInputLetters(letters);
        levelView.SetWordsRemaining(level.GetRemainingWords().Count);
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
                level.AddFoundWord(word);
                int wordsRemaining = level.GetRemainingWords().Count;
                levelView.AddCorrectWord(word);
                levelView.DisplayMessage("You found a correct word!");
                levelView.SetWordsRemaining(wordsRemaining);
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

    public void Scramble(){
        level.ScrambleLetters();
        SetLettersText(level.Letters);
        levelView.ScrambleInput();
    }

    void SetLettersText(List<char> letters){
        string lettersText = string.Format("Letters: {0}", new string(letters.ToArray()));
        levelView.SetLettersText(lettersText);
    }
}
