using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level
{
    public List<string> correctWords = new List<string>();
    public List<string> foundWords = new List<string>();

    public List<char> Letters
    {
        get {return constraint.letters;}
    }
    public string RegexDefinition
    {
        get {return constraint.regexDefinition;}
    }
    private Constraint constraint;
    private Dictionary dictionary;

    public enum WordValidity {Found, Correct, Bonus, Real, Fake};

    public Level(string dictionaryPath){
        dictionary = new Dictionary(dictionaryPath);
    }

    // Initalizes the level to a random word and constraint
    public void InitalizeRandom(){
        foundWords = new List<string>();
        correctWords = new List<string>();
        int attempts = 0;
        while(true){
            attempts++;
            string word = dictionary.RandomWord(7);
            string regex = GetRegex(word);
            List<char> listWord = new List<char>(word.ToCharArray());
            constraint = new Constraint(regex, listWord);
            correctWords = dictionary.GetMatchingConstraint(constraint);
            if(correctWords.Count > 5){
                break;
            }
        }
    }

    string GetRegex(string word){
        string regex = "^";
        int length = word.Length;
        int firstBound = length/2;
        int secondBound = length;

        int firstIndex = Random.Range(0, firstBound);
        int secondIndex = Random.Range(firstBound, secondBound);

        if(firstIndex > 0){
            regex += ".*";
        }
        regex += word[firstIndex];
        if(secondIndex > firstIndex + 1){
            regex += ".*";
        }
        regex += word[secondIndex];
        if(secondIndex < length - 1){
            regex += ".*"; // This could be optional
        }
        regex += "$";
        return regex;
    }

    public void ScrambleLetters(){
        constraint.ScrambleLetters();
    }
    public void AddFoundWord(string word){
        foundWords.Add(word);
    }

    public List<string> GetRemainingWords(){
        return correctWords.FindAll(e => !foundWords.Contains(e));
    }

    public WordValidity GetValidity(string word){
        if(IsWordFound(word)){
            return WordValidity.Found;
        }
        else if(IsWordCorrect(word)){
            return WordValidity.Correct;
        }
        else if(IsWordReal(word)){
            if(constraint.HasCorrectLetters(word)){
                return WordValidity.Bonus;
            }
            else{
                return WordValidity.Real;
            }
        }
        else{
            return WordValidity.Fake;
        }
    }

    // Checks if a word has already been found
    private bool IsWordFound(string word){
        return foundWords.Contains(word);
    }

    // Checks if a word matches the constraints
    private bool IsWordCorrect(string word){
        return correctWords.Contains(word);
    }

    // Checks if a word is real (in the dictionary)
    private bool IsWordReal(string word){
        return dictionary.IsValidWord(word);
    }
}
