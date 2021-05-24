using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class WordPanelScaler : MonoBehaviour, ILayoutSelfController
{
    public float maxScale = 1.5f;
    public RectTransform childRectTransform;
    RectTransform rectTransform;
    public LayoutGroup layoutGroup;
    float scaleFactor = 0.99f;
    public bool scale = false;
    // Keep track of the size for which the current scale and rect are valid
    // Prevents us from trying to ScaleUp every frame when we're good
    public Vector2 validForSize;
    public float validForWidth;
    
    void Start(){
        SetupReferences();
    }

    void SetupReferences(){
        rectTransform = GetComponent<RectTransform>();
    }

    void ScaleDown(){
        while(layoutGroup.minWidth > childRectTransform.rect.size.x){
            childRectTransform.localScale *= scaleFactor;
            FitToParent();
            // We have to rebuild the layout because the FlowLayout might move stuff around as we scale
            // and we need to take that into account on the next iteration
            LayoutRebuilder.ForceRebuildLayoutImmediate(childRectTransform);
        }
    }

    void ScaleUp(){

        Vector2 startScale = childRectTransform.localScale;
        while(layoutGroup.minWidth < childRectTransform.rect.size.x && childRectTransform.localScale.x < maxScale){
            startScale = childRectTransform.localScale;
            childRectTransform.localScale *=  1 + (1 - scaleFactor);
            FitToParent();
            
            // We have to rebuild the layout because the FlowLayout might move stuff around as we scale
            // and we need to take that into account on the next iteration
            LayoutRebuilder.ForceRebuildLayoutImmediate(childRectTransform);
        }
        // The last iteration will set us over so we need to downsize to fit again
        if(layoutGroup.minWidth > childRectTransform.rect.size.x){
            childRectTransform.localScale = startScale;
            FitToParent();
            validForSize = childRectTransform.rect.size;
            validForWidth = layoutGroup.minWidth;
        }
    }

    void FitToParent(){
        Vector2 parentRectSize = rectTransform.rect.size;
        Vector2 scaleRatio  = parentRectSize / (parentRectSize * childRectTransform.localScale);
        Vector2 newRectSize = parentRectSize * scaleRatio;
        Vector2 delta = newRectSize - parentRectSize;

        childRectTransform.sizeDelta = delta;
    }

    void Update(){
        //FindBestScale();
    }

    void FindBestScale(){
        
        // If either childRectTransform.rect.size or layoutGroup.minWidth change than we need to re-layout
        bool alreadyValidated = childRectTransform.rect.size == validForSize && layoutGroup.minWidth == validForWidth;
        
        if(!alreadyValidated){
            LayoutRebuilder.ForceRebuildLayoutImmediate(childRectTransform);
            FitToParent();
            if(layoutGroup.minWidth > childRectTransform.rect.size.x){
                ScaleDown();
            }
            else if(layoutGroup.minWidth < childRectTransform.rect.size.x && childRectTransform.localScale.x < maxScale){                    
                ScaleUp();
            }
        }
    }

    public void SetLayoutHorizontal(){
        FindBestScale();
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
