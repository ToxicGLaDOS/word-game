using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scales the childRectTransform to fit within this RectTransform
public class WordPanelScaler : MonoBehaviour, ILayoutSelfController
{
    [Tooltip("The maximum amount to scale up to")]
    public float maxScale = 1.5f;

    [Tooltip("How much to scale by each iteration. Closer to 1 will result in more computation but better results")]
    [Range(0, 1)]
    public float scaleFactor = 0.99f;

    [Tooltip("The child's rect transform that we are controlling")]
    public RectTransform childRectTransform;
    RectTransform rectTransform;
    LayoutGroup layoutGroup;
    
    void Start(){
        SetupReferences();
    }

    void SetupReferences(){
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = childRectTransform.GetComponent<LayoutGroup>();
    }

    // At least for my applications, ScaleDown seems to never be able to produce an infinite loop
    // There might be some edge case LayoutGroups that _could_, but this is good enough for me
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
            float newScaleX = Mathf.Min(childRectTransform.localScale.x * 1 + (1 - scaleFactor), maxScale);
            float newScaleY = Mathf.Min(childRectTransform.localScale.y * 1 + (1 - scaleFactor), maxScale);

            childRectTransform.localScale =  new Vector2(newScaleX, newScaleY);
            FitToParent();
            
            // We have to rebuild the layout because the FlowLayout might move stuff around as we scale
            // and we need to take that into account on the next iteration
            LayoutRebuilder.ForceRebuildLayoutImmediate(childRectTransform);
        }
        // The last iteration will set us over so we need to downsize to fit again
        if(layoutGroup.minWidth > childRectTransform.rect.size.x){
            childRectTransform.localScale = startScale;
            FitToParent();
        }
    }

    void FitToParent(){
        Vector2 parentRectSize = rectTransform.rect.size;
        Vector2 scaleRatio  = parentRectSize / (parentRectSize * childRectTransform.localScale);
        Vector2 newRectSize = parentRectSize * scaleRatio;
        Vector2 delta = newRectSize - parentRectSize;

        childRectTransform.sizeDelta = delta;
    }

    void FindBestScale(){
        // It's unclear whether this line is neccesarry or not. It seems to be needed if Update is driving
        // but when SetLayout is driving it might already have been called :shrug:
        // Leaving this comment just in case I see some weirdness
        //LayoutRebuilder.ForceRebuildLayoutImmediate(childRectTransform);
        if(layoutGroup.minWidth > childRectTransform.rect.size.x){
            ScaleDown();
        }
        else if(layoutGroup.minWidth < childRectTransform.rect.size.x && childRectTransform.localScale.x < maxScale){                    
            ScaleUp();
        }
        else{
            FitToParent();
        }
    }

    public void SetLayoutHorizontal(){
        FindBestScale();
    }

    public void SetLayoutVertical(){
        // We do everything in SetLayoutHorizontal
        //
        // If didn't want to scale evenly between x and y then
        // it might make sense to scale x and y seperatly and use this
        // function, but that's not what i'm using this for
    }

    // We have to set up the references here so that it works in the editor
    void OnValidate(){
        SetupReferences();
    }
}