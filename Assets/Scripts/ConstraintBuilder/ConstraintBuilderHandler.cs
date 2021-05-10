using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstraintBuilderHandler : MonoBehaviour
{
    private Dictionary dictionary;
    public Text numMatchesTextbox;
    public Text matchesTextbox;
    public InputField lettersInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMatches(InputField inputField){
        string regex = inputField.text;
        List<char> letters = new List<char>(lettersInput.text);
        Constraint constraint = new Constraint(regex, letters);
        List<string> matches = dictionary.GetMatchingConstraint(constraint);
        numMatchesTextbox.text = string.Format("{0} matches", matches.Count);


        string matchesText = "";

        foreach (string word in matches){
            matchesText += word + "\n";
        }

        matchesTextbox.text = matchesText;
    }
    public void ValidateWord(InputField inputField){
        string word = inputField.text;
        bool isValidWord = dictionary.IsValidWord(word);
        print(isValidWord);
    }
}
