using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Constraint
{
    public Regex regex;
    public string regexDefinition;
    public string letters;

    public Constraint(string regexDefinition, string availableLetters){
        regex = new Regex(regexDefinition, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        this.regexDefinition = regexDefinition;
        letters = availableLetters;
    }

    public bool HasCorrectLetters(string word){
        List<char> wordList = new List<char>(word.ToCharArray());
        List<char> lettersList = new List<char>(letters.ToCharArray());

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
        List<char> list = new List<char>(letters.ToCharArray());
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0,n + 1);
            char value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        letters = new string(list.ToArray());
    }
}
