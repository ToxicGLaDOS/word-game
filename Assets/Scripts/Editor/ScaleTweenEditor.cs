using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScaleTween))]
public class ScaleTweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ScaleTween scaleTween = (ScaleTween)target;
        if(GUILayout.Button("Set target scale")){
            scaleTween.SetTargetScale();
        }
    }
}
