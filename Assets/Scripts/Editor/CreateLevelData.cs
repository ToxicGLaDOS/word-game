using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class CreateLevelData : EditorWindow
{
    int numLetters;
    int minValid;
    int maxValid;
    int folderNameIndex;
    List<string> folderNames = new List<string>();
    Level level;
    int value;

    [MenuItem("Assets/Create/Level")]
    public static void OpenWindow(){
        EditorWindow.GetWindow(typeof(CreateLevelData));
    }

    void Awake(){
        level = new Level(GlobalValues.dictionaryPath);
        foreach(string path in Directory.GetDirectories("Assets/LevelData")){
            folderNames.Add(Path.GetFileName(path));
        }
    }

    void CreateScriptableObject(){
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        level.InitalizeFromCriteria(numLetters, minValid, maxValid);
        levelData.word = new string(level.Letters.ToArray());
        levelData.regexDefinition = level.RegexDefinition;
        int newLevelNumber = 0;
        foreach(string path in Directory.GetFiles("Assets/LevelData/" + folderNames[folderNameIndex])){
            // Watch out for those .meta files
            if(Path.GetExtension(path) == ".asset"){
                string file = Path.GetFileNameWithoutExtension(path);
                int levelNumber = int.Parse(file);
                if (levelNumber >= newLevelNumber){
                    newLevelNumber = levelNumber + 1;
                }
            }
        }
        AssetDatabase.CreateAsset(levelData, string.Format("Assets/LevelData/Mercury/{0}.asset", newLevelNumber));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = levelData;

    }

    void OnGUI(){
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        float fLetters = numLetters;
        numLetters = (int)EditorGUILayout.Slider("Num Letters", fLetters, 3, 10);
        EditorGUILayout.LabelField("Min: ", minValid.ToString());
        EditorGUILayout.LabelField("Max: ", maxValid.ToString());
        float fmin = minValid;
        float fmax = maxValid;
        EditorGUILayout.MinMaxSlider(ref fmin, ref fmax, 0, 40);
        minValid = (int)fmin;
        maxValid = (int)fmax;
        EditorGUILayout.Popup("Folder Name", folderNameIndex, folderNames.ToArray(), EditorStyles.popup);

        if(GUILayout.Button("Create")){
            CreateScriptableObject();
        }
    }
}
