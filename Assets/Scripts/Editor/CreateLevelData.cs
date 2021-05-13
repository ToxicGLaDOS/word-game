using System.Collections;
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
    int minValid;
    int maxValid = 40;
    int folderNameIndex;
    List<string> folderNames = new List<string>();
    Level level;
    int value;

    string levelDataPath = "Assets/Resources/LevelData";

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
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
        bool success = level.InitalizeFromCriteria(numLetters, minValid, maxValid);
        if(!success){
            Debug.Log("Failed to find word with those critera.");
            return;
        }
        levelData.word = new string(level.Letters.ToArray());
        levelData.regexDefinition = level.RegexDefinition;
        int newLevelNumber = 0;
        foreach(string path in Directory.GetFiles(levelDataPath + "/" + folderNames[folderNameIndex])){
            // Watch out for those .meta files
            if(Path.GetExtension(path) == ".asset"){
                string file = Path.GetFileNameWithoutExtension(path);
                int levelNumber = int.Parse(file);
                if (levelNumber >= newLevelNumber){
                    newLevelNumber = levelNumber + 1;
                }
            }
        }
        AssetDatabase.CreateAsset(levelData, string.Format(levelDataPath + "/Mercury/{0}.asset", newLevelNumber));
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
        //UnityEventTools.AddPersistentListener(button.onClick, buttonFunction);
        //prefab.transform.GetComponentInChildren<Button>().onClick.AddListener( () => buttonFunction(levelData));
        prefab.transform.GetComponentInChildren<Text>().text = name;
    }

    void RefreshLevelSelectPrefabs(){
        MainMenuView mainMenuView = FindObjectOfType<MainMenuView>();
        if (mainMenuView == null){
            Debug.Log("Couldn't find main menu view in hierarchy. Ensure you're in the correct scene");
        }
        UnityAction<LevelData> levelSelectAction = new UnityAction<LevelData>(mainMenuView.SelectLevel);
        if(levelSelectAction == null){
            Debug.Log("Couldn't find function to select level on interatction handler.");
        }
        GameObject levelSelect = GameObject.Find("LevelSelect");
        if (levelSelect == null){
            Debug.Log("Couldn't find level select object in hierarchy. Perhaps you're on the wrong scene");
            return;
        }
        GameObject content = levelSelect.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        if (content.name != "Content"){
            Debug.Log("LevelSelect hierarchy is not what was expected");
            return;
        }
        GameObject panel = content.transform.Find(folderNames[folderNameIndex]).gameObject;
        // Create a copy of the children list because we can't
        // iterate over it and destroy at the same time
        List<Transform> panelChildren = new List<Transform>();
        foreach(Transform child in panel.transform){
            panelChildren.Add(child);
        }
        foreach(Transform child in panelChildren){
            DestroyImmediate(child.gameObject);
        }

        foreach(string path in Directory.GetFiles(levelDataPath + "/" + folderNames[folderNameIndex])){
            // Watch out for those .meta files
            if(Path.GetExtension(path) == ".asset"){
                
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

                CreateLevelSelectPrefab(levelNumber.ToString(), panel.transform, levelSelectAction, levelData);
            }
        }
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
            RefreshLevelSelectPrefabs();
        }
        if(GUILayout.Button("Refresh Prefabs")){
            RefreshLevelSelectPrefabs();
        }
    }
}
