using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.Events;
using UnityEngine.Events;

public class ListenerTest : MonoBehaviour
{
    [MenuItem("Assets/Create/Listener Test")]
    public static void Listener(){
        MainMenuView mainMenuView = FindObjectOfType<MainMenuView>();
        GameObject go = GameObject.Find("TestButton");
        LevelData ld = Resources.Load<LevelData>("LevelData/Mercury\\0");
        if (ld == null){
            Debug.Log("ld is null");
            return;
        }
        Debug.Log(ld.word);
        GameObject paramGo = GameObject.Find("TestGo");
        Button button = go.GetComponent<Button>();

        UnityAction<LevelData> buttonFunction = new UnityAction<LevelData>(mainMenuView.Test);

        UnityEventTools.AddObjectPersistentListener<LevelData>(button.onClick, buttonFunction, ld);
    }
}
