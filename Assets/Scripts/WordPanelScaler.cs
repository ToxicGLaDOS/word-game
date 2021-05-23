using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class WordPanelScaler : MonoBehaviour, ILayoutSelfController
{

    RectTransform rectTransform;
    RectTransform parentRectTransform;
    LayoutGroup layoutGroup;
    float scaleFactor = 0.99f;
    
    void Start(){
        SetupReferences();
    }

    void SetupReferences(){
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<LayoutGroup>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    void ScaleDown(){
        rectTransform.localScale *= scaleFactor;
    }

    void FitToParent(){
        Vector2 parentRectSize = parentRectTransform.rect.size;
        Vector2 scaleRatio  = parentRectSize / (parentRectSize * rectTransform.localScale);
        Vector2 newRectSize = parentRectSize * scaleRatio;
        Vector2 delta = newRectSize - parentRectSize;

        rectTransform.sizeDelta = delta;
    }

    public void SetLayoutHorizontal(){
        FitToParent();
        while(layoutGroup.minWidth > rectTransform.rect.size.x){
            ScaleDown();
            FitToParent();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); // Fixes weirdness where adding words doesn't update the layout
        }
    }

    public void SetLayoutVertical(){
        FitToParent();
        while(layoutGroup.minWidth > rectTransform.rect.size.x){
            ScaleDown();
            FitToParent();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); // Fixes weirdness where adding words doesn't update the layout
        }
    }

    // We have to set up the references here so that it works in the editor
    void OnValidate(){
        SetupReferences();
    }
}
