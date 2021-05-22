using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordPanelLayout : LayoutGroup
{
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        SetLayoutInputForAxis(500, 500, 500, 0);
    }

    // The point of CalculateLayoutInput{Horizontal/Vertical} methods
    // is to call SetLayoutInputForAxis to set the min, preferred, and flexible
    // {height/width} for this element
    public override void CalculateLayoutInputVertical()
    {
        if(rectChildren.Count > 0){
            float minHeight   = LayoutUtility.GetMinHeight(rectChildren[0])       * rectChildren.Count;
            float prefHeight  = LayoutUtility.GetPreferredHeight(rectChildren[0]) * rectChildren.Count;
            float flexHeight  = LayoutUtility.GetFlexibleHeight(rectChildren[0])  * rectChildren.Count;

            SetLayoutInputForAxis(minHeight, prefHeight, flexHeight, 1);
        }
    }

    public override void SetLayoutHorizontal()
    {
        foreach(RectTransform child in rectChildren){
            SetChildAlongAxis(child, 0, 0);
        }
    }

    // The point of SetLayout{Horizontal/Vertical} methods
    // is to call SetChildAlongAxis for each child in rectChildren
    // to set the childs position and size along the axis
    public override void SetLayoutVertical()
    {
        print(rectTransform.rect.height);
        for(int i = 0; i < rectChildren.Count; i++){
            RectTransform child = rectChildren[i];
            print(LayoutUtility.GetMinHeight(child));
            SetChildAlongAxisWithScale(child, 1, 30 * i, 10, 1);
        }
    }
}
