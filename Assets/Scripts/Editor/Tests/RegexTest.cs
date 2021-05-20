using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RegexTest : EditorWindow
{
    string word;
    bool startWithFirstLetter;
    bool endWithLastLetter;
    int setLetters;
    string regex;
    [MenuItem("Tests/Regex")]
    public static void OpenWindow(){
        EditorWindow.GetWindow(typeof(RegexTest));
    }

    void OnGUI(){
        EditorGUILayout.LabelField("Regex text", EditorStyles.boldLabel);

        word = EditorGUILayout.TextField("Word:", word);
        startWithFirstLetter = EditorGUILayout.Toggle("Start with first letter of word", startWithFirstLetter);
        endWithLastLetter = EditorGUILayout.Toggle("End with last letter of word", endWithLastLetter);
        setLetters = EditorGUILayout.IntField("Num set letters", setLetters);
        if(GUILayout.Button("Generate Regex")){
            RegexBuilderOptions regexOptions = new RegexBuilderOptions(setLetters, startWithFirstLetter, endWithLastLetter);
            regex = RegexBuilder.GetRegex(word, regexOptions);
        }

        if(regex != null){
            GUILayout.Label(string.Format("Regex: {0}", regex));
        }
    }

}
