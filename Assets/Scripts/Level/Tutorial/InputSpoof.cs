using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputSpoof : MonoBehaviour
{
    public List<InputsCommand> commands = new List<InputsCommand>();
    GameObject entered;
    bool valuesDirty = false;
    bool _mouseButtonUp = false;
    bool _mouseButtonDown = false;

    public bool MouseButtonUp{
        get{
            return _mouseButtonUp;
        }
    }
    public bool mouseButtonDown{
        get{
            return _mouseButtonDown;
        }
    }
    MouseState _mouseButtonState = MouseState.UP;
    public MouseState MouseButtonState {
        get{
            return _mouseButtonState;
        }

        private set{
            if(_mouseButtonState != value){
                _mouseButtonState = value;
                if(value == MouseState.UP){
                    _mouseButtonUp = true;
                    print("Set _mouseButtonUp to true");
                }
                else{
                    _mouseButtonDown = true;
                }
                valuesDirty = true;
            }
        }
    }

    public enum MouseState {UP, DOWN};
    int curCommand = 0;
    RectTransform rectTransform;
    static int _pointerID;
    public static int PointerID{
        get{
            return -1000;
        }
    }
    // Start is called before the first frame update

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        commands[curCommand].Execute(rectTransform);
    }

    void Update(){
        List<RaycastResult> results = GetRaycastResults();
        RaycastResult firstResult = FindFirstRaycast(results);
        if(firstResult.gameObject != null && firstResult.gameObject != entered){
            ExecuteEvents.ExecuteHierarchy<IPointerEnterHandler>(firstResult.gameObject, GetPointerEventData(), ExecuteEvents.pointerEnterHandler);
            if(entered != null){
                ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(entered, GetPointerEventData(), ExecuteEvents.pointerExitHandler);
            }
            entered = firstResult.gameObject;
        }
        if(!valuesDirty){
            _mouseButtonDown = false;
            _mouseButtonUp = false;
            valuesDirty = false;
        }

    }

    public void ClickDown(){
        List<RaycastResult> results = GetRaycastResults();
        RaycastResult result = FindFirstRaycast(results);
        if(result.gameObject != null){
            ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(result.gameObject, GetPointerEventData(), ExecuteEvents.pointerDownHandler);
        }
        MouseButtonState = MouseState.DOWN;
    }

    public void ClickUp(){
        List<RaycastResult> results = GetRaycastResults();
        RaycastResult result = FindFirstRaycast(results);
        if(result.gameObject != null){
            ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(result.gameObject, GetPointerEventData(), ExecuteEvents.pointerUpHandler);
        }
        MouseButtonState = MouseState.UP;
    }

    // Creates a PointerEventData with the correct PointerID
    PointerEventData GetPointerEventData(){
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.pointerId = PointerID;
        return pointerEventData;
    }

    List<RaycastResult> GetRaycastResults(){
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        Vector3 cursorPosition = Camera.main.WorldToScreenPoint(transform.position);
        graphicRaycaster.Raycast(new PointerEventData(EventSystem.current) { position = cursorPosition }, results);
        return results;
    }

    RaycastResult FindFirstRaycast(List<RaycastResult> results){
        foreach(RaycastResult result in results){
            if(result.gameObject != gameObject){
                return result;
            }
        }
        return new RaycastResult();
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