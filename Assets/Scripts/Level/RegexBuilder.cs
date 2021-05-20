using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public struct RegexBuilderOptions{
    public int setLetters;
    public bool startWithFirstLetter;
    public bool endWithLastLetter;

    public RegexBuilderOptions(int setLetters, bool startWithFirstLetter = false, bool endWithLastLetter = false){
        this.setLetters = setLetters;
        this.startWithFirstLetter = startWithFirstLetter;
        this.endWithLastLetter = endWithLastLetter;
    }

}
public class RegexBuilder
{
    public static string GetRegex(string word, RegexBuilderOptions regexOptions){
        bool startWithFirstLetter = regexOptions.startWithFirstLetter;
        bool endWithLastLetter = regexOptions.endWithLastLetter;
        int setLetters = regexOptions.setLetters;

        int startEndRequirements = (startWithFirstLetter ? 1 : 0) + (endWithLastLetter ? 1 : 0);
        if (startEndRequirements > setLetters){
            throw new System.Exception(string.Format("Start and end settings require at least {0} setLetters, but setLetters was {1}", startEndRequirements, setLetters));
        }
        if (word.Length < setLetters){
            throw new System.Exception(string.Format("Word \"{0}\" only has {1} letters. But setLetters was more than that ({2}).", word, word.Length, setLetters));
        }
        
        // Seperate each letter in word into a list
        List<string> regexComponents = new List<string>();
        foreach(char letter in word.ToCharArray()){
            regexComponents.Add(letter.ToString());
        }
        string regex = "^";

        // Ensure we can't remove the start or end letters
        // if the respective variable is set
        int startIndex = startWithFirstLetter ? 1 : 0;
        int endIndex = endWithLastLetter ? word.Length - 1 : word.Length;

        // While the number of letters is greater than setLetters
        // Pick a random component and set it to ".*"
        for(int i = 0; i < word.Length - setLetters; i++){
            List<int> validLetters = regexComponents.FindAllIndexes( s => s != ".*");

            // Remove first and last letters from the list of
            // letters that are valid to turn into ".*" if necessary
            if(startWithFirstLetter){
                validLetters = validLetters.Skip(1).ToList();
            }
            if(endWithLastLetter){
                validLetters = validLetters.Take(validLetters.Count - 1).ToList();
            }
            int index = validLetters[Random.Range(0, validLetters.Count)];
            regexComponents[index] = ".*";
        }
        // If we don't care what the first letter is than we can allow any letter to start
        if(!startWithFirstLetter){
            regexComponents.Insert(0, ".*");
        }
        // Allow the regex to end with anything if endWithLastLetter is false
        if(!endWithLastLetter){
            regexComponents.Add(".*");
        }
        
        regex += CreateRegexFromComponents(regexComponents);

        // If we don't care what the last letter is than allow it to end with anything
        regex += "$";
        return regex;
    }

    // Dedupes ".*" and combines back into string form
    private static string CreateRegexFromComponents(List<string> components){
        string regex = "";
        foreach(string component in components){
            if(component != ".*"){
                regex += component;
            }
            else if(!regex.EndsWith(".*")){
                regex += ".*";
            }
        }
        return regex;
    }
}

public static class ListExtensions{
    public static List<int> FindAllIndexes(this List<string> strings, System.Predicate<string> predicate){
        List<int> indexes = new List<int>();
        for(int i = 0; i < strings.Count; i++){
            if(predicate.Invoke(strings[i])){
                indexes.Add(i);
            }
        }

        return indexes;
    }
}
