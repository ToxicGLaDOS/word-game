using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTween : MonoBehaviour
{
    public Vector3 targetScale;
    [Range(0, 10)]
    public float duration;
    public LeanTweenType easeType;
    public UnityEvent onStartCallback;
    public UnityEvent onCompleteCallback;
    public GameObject target;
    public bool activate;
    public void Tween(){
        if(onStartCallback != null){
            onStartCallback.Invoke();
        }
        LeanTween.scale(target, targetScale, duration).setEase(easeType).setOnComplete(OnComplete);
    }

    public void OnComplete(){
        if(onCompleteCallback != null){
            onCompleteCallback.Invoke();
        }
    }

    public void SetTargetScale(){
        targetScale = transform.localScale;
    }
}
