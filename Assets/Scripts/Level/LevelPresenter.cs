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

        levelView.SetRegexText(regexText);
        levelView.SetInputLetters(letters);
        levelView.InitalizeWords(level.GetRemainingWords());
        levelView.AnimateUI();

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
        LevelData nextLevelData = Resources.Load<LevelData>(string.Format("LevelData/{0}/{1}", GlobalValues.levelData.groupData.name, nextLevelIndex.ToString()));
        
        if(nextLevelData != null){
            int playersCurrentGroup = PlayerPrefs.GetInt("CurrentGroup", 0);
            int playersCurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
            // If we completed a level from the last group the player has unlocked
            // and the level is the last level the player has unlocked
            // then unlock the next level
            if(playersCurrentGroup == nextLevelData.groupData.index && playersCurrentLevel + 1 == nextLevelIndex){
                int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
                PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            }
            GlobalValues.levelData = nextLevelData;
            Initalize();
        }
        else{
            GroupData nextGroup = GlobalValues.levelData.groupData.next;
            if(nextGroup != null){
                int playersCurrentGroup = PlayerPrefs.GetInt("CurrentGroup", 0);
                // If the the next group is further than the player has gotten
                // then unlock the next group and set the leve back to 0
                if(nextGroup.index == playersCurrentGroup + 1){
                    PlayerPrefs.SetInt("CurrentGroup", nextGroup.index);
                    PlayerPrefs.SetInt("CurrentLevel", 0); // Reset back to level 0 for next group
                }
                else if(nextGroup.index > playersCurrentGroup + 1){
                    Debug.LogError(string.Format("Tried to load a group further than the next one. GroupData index might be incorrect. Culprits were nextGroup = {0} and playersCurrentGroup = {1}", nextGroup, playersCurrentGroup));
                }

                GlobalValues.levelData = Resources.Load<LevelData>(string.Format("LevelData/{0}/0", nextGroup.name));
                Initalize();
            }
            else{
                // TODO: Generate level from here
                Debug.Log("Out of levels.");
            }
        }
    }

    public void Solve(){
        foreach(string word in level.correctWords){
            CorrectWord(word);
        }
    }

    public void Scramble(){
        level.ScrambleLetters(); // TODO: Pretty sure this is unnecessary now
        levelView.ScrambleInput();
    }
}
