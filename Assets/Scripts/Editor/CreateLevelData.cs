﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEditor.Events;

public class CreateLevelData : EditorWindow
{
    public GameObject levelSelectOptionPrefab;
    int numLetters;
    int minValid = 3;
    int maxValid = 40;
    int folderNameIndex;
    List<string> folderNames = new List<string>();
    Level level;
    int value;

    string levelDataPath = "Assets/Resources/LevelData";
    string groupDataResourcePath = "GroupData";

    [MenuItem("Assets/Create/Level")]
    public static void OpenWindow(){
        EditorWindow.GetWindow(typeof(CreateLevelData));
    }

    void Awake(){
        level = new Level(GlobalValues.dictionaryPath);
        foreach(string path in Directory.GetDirectories(levelDataPath)){
            folderNames.Add(Path.GetFileName(path));
        }
    }

    void CreateScriptableObject(){
        string folderName = folderNames[folderNameIndex];
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        bool success = level.InitalizeFromCriteria(numLetters, minValid, maxValid);
        if(!success){
            Debug.Log("Failed to find word with those critera.");
            return;
        }
        levelData.word = new string(level.Letters.ToArray());
        levelData.regexDefinition = level.RegexDefinition;
        levelData.groupData = Resources.Load<GroupData>(groupDataResourcePath + "/" + folderName);
        int newLevelNumber = 0;
        foreach(string path in Directory.GetFiles(levelDataPath + "/" + folderName)){
            // Watch out for those .meta files
            if(Path.GetExtension(path) == ".asset"){
                string file = Path.GetFileNameWithoutExtension(path);
                int levelNumber = int.Parse(file);
                if (levelNumber >= newLevelNumber){
                    newLevelNumber = levelNumber + 1;
                }
            }
        }
        AssetDatabase.CreateAsset(levelData, string.Format(levelDataPath + "/" + folderName + "/{0}.asset", newLevelNumber));
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = levelData;

    }

    void CreateLevelSelectPrefab(string name, Transform parentTransform, UnityAction<LevelData> buttonFunction, LevelData levelData){
        GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(levelSelectOptionPrefab);
        prefab.name = name;
        Button button = prefab.GetComponentInChildren<Button>();

        prefab.transform.SetParent(parentTransform, false);
        prefab.transform.localPosition = Vector3.zero;
        prefab.transform.localEulerAngles = Vector3.zero;
        prefab.transform.localScale = Vector3.one;

        UnityEventTools.AddObjectPersistentListener<LevelData>(button.onClick, buttonFunction, levelData);
        prefab.transform.GetComponentInChildren<Text>().text = name;
    }

    void RefreshLevelSelectPrefabs(){
        MainMenuView mainMenuView = FindObjectOfType<MainMenuView>();
        if (mainMenuView == null){
            Debug.LogWarning("Couldn't find main menu view in hierarchy. Ensure you're in the correct scene");
            return;
        }
        UnityAction<LevelData> levelSelectAction = new UnityAction<LevelData>(mainMenuView.SelectLevel);
        if(levelSelectAction == null){
            Debug.LogWarning("Couldn't find function to select level on interatction handler.");
            return;
        }
        GameObject levelSelect = GameObject.Find("LevelSelect");
        if (levelSelect == null){
            Debug.LogWarning("Couldn't find level select object in hierarchy. Perhaps you're on the wrong scene");
            return;
        }
        GameObject content = levelSelect.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        if (content.name != "Content"){
            Debug.LogWarning("LevelSelect hierarchy is not what was expected");
            return;
        }
        GameObject panel = content.transform.Find(folderNames[folderNameIndex]).gameObject;
        GameObject levels = FindDeepChild(panel.transform, "Levels").gameObject;
        // Create a copy of the children list because we can't
        // iterate over it and destroy at the same time
        List<Transform> levelsChildren = new List<Transform>();
        foreach(Transform child in levels.transform){
            levelsChildren.Add(child);
        }
        foreach(Transform child in levelsChildren){
            DestroyImmediate(child.gameObject);
        }
        List<string> paths = GetSortedPaths();
        foreach(string path in paths){
            string file = Path.GetFileNameWithoutExtension(path);
            int levelNumber = int.Parse(file);

            // We have to mangle the path because Resources.Load()
            // expects the path to be relative to the Resources folder
            string resourcePath = path.Replace("Assets/Resources/", "");
            resourcePath = Path.GetDirectoryName(resourcePath);
            resourcePath = Path.Combine(resourcePath, file);
            LevelData levelData = Resources.Load<LevelData>(resourcePath);
            if(levelData == null){
                Debug.LogWarning(string.Format("Failed to load levelData at path \"{0}\", skipping", resourcePath));
                continue;
            }

            CreateLevelSelectPrefab(levelNumber.ToString(), levels.transform, levelSelectAction, levelData);
        }
    }

    public Transform FindDeepChild(Transform parentTransform, string childName){
        Transform correctChild = parentTransform.Find(childName);
        if(correctChild != null){
            return correctChild;
        }
        else{
            foreach(Transform child in parentTransform){
                correctChild = FindDeepChild(child, childName);
                if(correctChild != null){
                    return correctChild;
                }
            }
        }
        return null;
    }

    List<string> GetSortedPaths(){
        List<string> paths = new List<string>();
        List<int> pathValues = new List<int>();
        foreach (string path in Directory.GetFiles(levelDataPath + "/" + folderNames[folderNameIndex])){
            // Don't include .meta files
            if(Path.GetExtension(path) == ".asset"){
                paths.Add(path);
                string file = Path.GetFileNameWithoutExtension(path);
                pathValues.Add(int.Parse(file));
            }
        }
        string[] pathsArray = paths.ToArray();
        Array.Sort(pathValues.ToArray(), pathsArray);
        paths = new List<string>(pathsArray);

        return paths;
    }

    void OnGUI(){
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        float fLetters = numLetters;
        numLetters = (int)EditorGUILayout.Slider("Num Letters", fLetters, 3, 10);
        EditorGUILayout.LabelField("Min: ", minValid.ToString());
        EditorGUILayout.LabelField("Max: ", maxValid.ToString());
        float fmin = minValid;
        float fmax = maxValid;
        EditorGUILayout.MinMaxSlider(ref fmin, ref fmax, 3, 40);
        minValid = (int)fmin;
        maxValid = (int)fmax;
        EditorGUILayout.Popup("Folder Name", folderNameIndex, folderNames.ToArray(), EditorStyles.popup);

        if(GUILayout.Button("Create")){
            CreateScriptableObject();
            RefreshLevelSelectPrefabs();
        }
        if(GUILayout.Button("Refresh Prefabs")){
            RefreshLevelSelectPrefabs();
        }
    }
}
