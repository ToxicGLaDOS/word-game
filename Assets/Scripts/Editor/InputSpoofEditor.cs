using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputSpoof))]
public class InputSpoofEditor : Editor
{
    InputSpoof inputSpoof;
    SerializedObject serializedSpoof;
    SerializedProperty commandList;
    void OnEnable(){
        inputSpoof = (InputSpoof)target;
        serializedSpoof = new SerializedObject(inputSpoof);
        commandList = serializedSpoof.FindProperty("commands");
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InputSpoof inputSpoof = (InputSpoof)target;
        if(GUILayout.Button("Create move command")){
            //CreateMoveCommand();
        }
    }

    //void CreateMoveCommand(){
    //    commandList.InsertArrayElementAtIndex(commandList.arraySize);
    //    MoveCommand command = new MoveCommand(inputSpoof.GetComponent<RectTransform>(), new Vector2(1,1), 1);
    //    inputSpoof.commands.Add(command);
    //}
}
