using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputSpoof : MonoBehaviour
{
    public List<InputsCommand> commands = new List<InputsCommand>();
    int curCommand = 0;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        for(int i = 0; i < commands.Count; i++){
            InputsCommand command = commands[i];
            // Not the last command
            if(i < commands.Count - 1){
                command.onComplete.AddListener(ExecuteNextCommand);
            }
            // Last command
            else{
                command.onComplete.AddListener(Restart);
            }
        }
        commands[curCommand].Execute(rectTransform);
    }

    public void ExecuteNextCommand(){
        curCommand++;
        commands[curCommand].Execute(rectTransform);
    }

    public void Restart(){
        curCommand = 0;
        commands[curCommand].Execute(rectTransform);
    }
}

[System.Serializable]
public struct InputsCommand {
    public Vector2 destination;
    [Range(0, 3)]
    public float duration;
    public UnityEvent onComplete;

    public void Execute(RectTransform cursor){
        LeanTween.move(cursor, destination, duration).setOnComplete(OnComplete);
    }

    void OnComplete(){
        if(onComplete != null){
            onComplete.Invoke();
        }
    }
}