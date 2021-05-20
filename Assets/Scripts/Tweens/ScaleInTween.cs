using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleInTween : MonoBehaviour
{
    public Vector3 startScale;
    [Range(0, 10)]
    public float duration;
    public LeanTweenType easeType;
    public UnityEvent onCompleteCallback;
    public void Tween(){
        Vector3 endScale = transform.localScale;
        transform.localScale = startScale;
        LeanTween.scale(gameObject, endScale, duration).setEase(easeType).setOnComplete(OnComplete);
    }

    public void OnComplete(){
        if(onCompleteCallback != null){
            onCompleteCallback.Invoke();
        }
    }
}
