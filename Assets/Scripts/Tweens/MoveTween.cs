using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveTween : MonoBehaviour
{
    public Vector3 endPosition;
    [Range(0, 10)]
    public float duration;
    public LeanTweenType easeType;
    public UnityEvent onCompleteCallback;
    public void Tween(){
        LeanTween.move(GetComponent<RectTransform>(), endPosition, duration).setEase(easeType).setOnComplete(OnComplete);
    }

    public void OnComplete(){
        if(onCompleteCallback != null){
            onCompleteCallback.Invoke();
        }
    }

    public void SetTargetPosition(){
        endPosition = GetComponent<RectTransform>().anchoredPosition;
    }

}
