using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Constraint
{
    public Regex regex;
    public string regexDefinition;
    public List<char> letters;

    public Constraint(string regexDefinition, List<char> availableLetters){
        regex = new Regex(regexDefinition, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        this.regexDefinition = regexDefinition;
        letters = availableLetters;
    }

    public bool HasCorrectLetters(string word){
        List<char> wordList = new List<char>(word.ToCharArray());
        List<char> lettersList = new List<char>(letters); // Copy the list because we remove from this list

        foreach(char wordChar in wordList){
            if(lettersList.Contains(wordChar)){
                lettersList.Remove(wordChar);
            }
            else{
                return false;
            }
        }

        return true;
    }



    public void ScrambleLetters(){
        int n = letters.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0,n + 1);
            char value = letters[k];
            letters[k] = letters[n];
            letters[n] = value;
        }
    }
}
