using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
public class Dictionary
{
    public string dictionaryPath;
    public List<string> dictionary = new List<string>();
    // Start is called before the first frame update
    public Dictionary(string dictionaryPath){
        LoadDictionary(dictionaryPath);
    }

    public bool IsValidWord(string word, int minLength = 3){
        bool contains = dictionary.Contains(word);
        return word.Length >= minLength && contains;
    }

    public int NumMatchingConstraint(Constraint constraint, int max = 10000){
        int count = 0;
        foreach (string word in dictionary){
            if(constraint.regex.IsMatch(word)){
                count++;
            }
            if (count >= max){
                break;
            }
        }

        return count;
    }

    public List<string> GetMatchingConstraint(Constraint constraint, int max = 10000){
        List<string> matches = new List<string>();
        foreach (string word in dictionary){
            if(constraint.regex.IsMatch(word) && constraint.HasCorrectLetters(word) && word.Length >= 3){
                matches.Add(word);
            }
            if(matches.Count >= max){
                break;
            }
        }

        return matches;
    }


    string RandomWord(){
        int index = Random.Range(0, dictionary.Count);
        return dictionary[index];
    }

    public string RandomWord(int length){
        List<string> filtered = dictionary.FindAll(e => e.Length == length);
        int index = Random.Range(0, filtered.Count);
        return filtered[index];
    }

    void LoadDictionary(string dictionaryPath){
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(dictionaryPath);  
        while((line = file.ReadLine()) != null)  
        {
            dictionary.Add(line);
        }  
        file.Close();  
    }
}
