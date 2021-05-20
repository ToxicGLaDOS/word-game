using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveTween))]
public class MoveTweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MoveTween moveTween = (MoveTween)target;
        if(GUILayout.Button("Set target position")){
            moveTween.SetTargetPosition();
        }
    }
}
