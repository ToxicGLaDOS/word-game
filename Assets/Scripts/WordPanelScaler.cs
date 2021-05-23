using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class WordPanelScaler : MonoBehaviour, ILayoutSelfController
{
    public float maxScale = 1.5f;
    RectTransform rectTransform;
    RectTransform parentRectTransform;
    LayoutGroup layoutGroup;
    float scaleFactor = 0.99f;
    bool scalingUp = false;
    // Keep track of the size for which the current scale and rect are valid
    // Prevents us from trying to ScaleUp every frame when we're good
    Vector2 validForSize;
    
    void Start(){
        SetupReferences();
    }

    void SetupReferences(){
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<LayoutGroup>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    void ScaleDown(){
        while(layoutGroup.minWidth > rectTransform.rect.size.x){
            rectTransform.localScale *= scaleFactor;
            FitToParent();
            // We have to rebuild the layout because the FlowLayout might move stuff around as we scale
            // and we need to take that into account on the next iteration
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    void ScaleUp(){
        while(layoutGroup.minWidth < rectTransform.rect.size.x && rectTransform.localScale.x < maxScale){
            rectTransform.localScale *=  1 + (1 - scaleFactor);
            FitToParent();
            // We have to rebuild the layout because the FlowLayout might move stuff around as we scale
            // and we need to take that into account on the next iteration
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    void FitToParent(){
        Vector2 parentRectSize = parentRectTransform.rect.size;
        Vector2 scaleRatio  = parentRectSize / (parentRectSize * rectTransform.localScale);
        Vector2 newRectSize = parentRectSize * scaleRatio;
        Vector2 delta = newRectSize - parentRectSize;

        rectTransform.sizeDelta = delta;
    }

    void Update(){
        FindBestScale();
    }

    void FindBestScale(){
        if(rectTransform.rect.size != validForSize){
            FitToParent();
            if(layoutGroup.minWidth > rectTransform.rect.size.x && !scalingUp){
                ScaleDown();
            }
            else if(layoutGroup.minWidth < rectTransform.rect.size.x && rectTransform.localScale.x < maxScale){
                print("Trying to scale up");
                Vector2 startScale = rectTransform.localScale;
                scalingUp = true;
                ScaleUp();
                // If scaling up put us over the limit than revert to startScale
                if(layoutGroup.minWidth > rectTransform.rect.size.x){
                    rectTransform.localScale = startScale;
                    FitToParent();
                    validForSize = rectTransform.rect.size;
                }
            }
        }
    }

    public void SetLayoutHorizontal(){
        FitToParent();
        //FindBestScale();
    }

    public void SetLayoutVertical(){
    }

    // We have to set up the references here so that it works in the editor
    void OnValidate(){
        SetupReferences();
    }
}

public static class Vector3Ext
{
    public static string Log(this Vector3 v)
    {
        return v.x + " " + v.y + " " + v.z;
    }
}
