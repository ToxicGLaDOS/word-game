using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public GameObject lineRendererPrefab;

    public void ClearAllLines(){
        foreach(Transform child in transform){
            child.GetComponent<LineRenderer>().positionCount = 0;
        }
    }

    public void AddStaticLine(Vector3 start, Vector3 end){
        LineRenderer lineRenderer = FindFreeLineRenderer();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] {start, end});
    }

    public void UpdateDynamicLine(Vector3 start, Vector3 end){
        Transform dynamicLineTransform = transform.Find("Dynamic");
        LineRenderer dynamicLine = dynamicLineTransform.GetComponent<LineRenderer>();
        dynamicLine.positionCount = 2;
        dynamicLine.SetPositions(new Vector3[] {start, end});
    }

    LineRenderer CreateNewLineRenderer(){
        GameObject go = Instantiate(lineRendererPrefab, transform);
        return go.GetComponent<LineRenderer>();
    }

    LineRenderer FindFreeLineRenderer(){
        foreach(Transform child in transform){
            if(child.name != "Dynamic"){
                LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
                if(lineRenderer.positionCount == 0){
                    return lineRenderer;
                }
            }
        }
        return CreateNewLineRenderer();
    }
}
