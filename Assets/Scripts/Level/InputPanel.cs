using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.UI.Extensions;
public class InputPanel : MonoBehaviour
{
    [Range(0, 500)]
    // This is the amount of padding the image has between
    // the true edge of the image and where the letters should be placed
    public float imagePadding;
    [Range(0, 1)]
    public float scrambleSpeed;
    public GameObject letterPrefab;
    ReadOnlyCollection<char> letters; // Read only because Constaint should handle mutation
    bool inputStarted = false; // Wether we are currently inputting a sequence of letters
    public List<GameObject> inputSequence = new List<GameObject>();
    public Transform inputLetters;
    UILineRenderer uiLineRenderer;
    LevelView levelView;
    List<Vector2> inputLetterStartingPositions = new List<Vector2>();
    List<Vector2> inputLetterTargetPositions = new List<Vector2>();

    private float Radius{
        get {
            return GetComponent<RectTransform>().rect.width / 2 - imagePadding;
        }
    }

    public string InputWord{
        get {
            string s = "";
            foreach(GameObject letterObj in inputSequence){
                s += char.Parse(letterObj.GetComponentInChildren<Text>().text);
            }
            return s;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiLineRenderer = GetComponentInChildren<UILineRenderer>();
        levelView = FindObjectOfType<LevelView>();
    }

    public void Initalize(List<char> letters){
        // Delete all children
        // If this is called after the input panel has been initalized
        // than we want to get rid of the old characters
        foreach (Transform child in inputLetters){
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        this.letters = letters.AsReadOnly();
        foreach (char character in letters){
            GameObject letter = Instantiate(letterPrefab, inputLetters);
            letter.GetComponentInChildren<Text>().text = character.ToString();
        }
        GroupData groupData = GlobalValues.levelData.groupData;
        Image image = GetComponent<Image>();
        image.sprite = groupData.panelSprite;

        foreach(Transform child in inputLetters){
            child.GetComponentInChildren<Text>().color = groupData.textColor;
        }

        PositionLetters();
    }

    void PositionLetters(){
        // The index of the current child counting only active children
        int childIndex = 0;
        foreach(Transform childTransform in inputLetters){
            // We need to count only the active children because
            // when initalizing another level the old children won't be destroyed yet
            // so we use the activeness to decided whether they're new or old
            if (childTransform.gameObject.activeInHierarchy){
                Vector2 position = GetPosition(childIndex);
                childTransform.localPosition = position;
                childIndex++;
            }
        }
    }

    int NumActiveChildren(){
        int activeChildCount = 0;
        foreach(Transform child in inputLetters){
            if (child.gameObject.activeInHierarchy){
                activeChildCount++;
            }
        }

        return activeChildCount;
    }

    Vector2 GetPosition(int index){
        int activeChildCount = NumActiveChildren();
        float theta = 2 * Mathf.PI / activeChildCount * index;
        float x = Radius * Mathf.Cos(theta);
        float y = Radius * Mathf.Sin(theta);

        return new Vector2(x, y);
    }

    // This function doesn't make any attempt to keep the
    // letters aligned with the order in the "letters" list
    public void Scramble(){
        // This works for now, but if we end up using more coroutines
        // in InputPanel than it will stop those too and that's probably
        // not what we want
        StopAllCoroutines();
        List<Transform> children = new List<Transform>();
        List<Vector2> targetPositions = new List<Vector2>();
        foreach(Transform child in inputLetters){
            children.Add(child);
        }
        for(int i = 0; i < children.Count; i++){
            targetPositions.Add(GetPosition(i));
        }
        for(int i = 0; i < children.Count; i++){
            Transform child = inputLetters.GetChild(i);
            Vector2 start = child.localPosition;

            // Pick random target position
            int index = Random.Range(0, targetPositions.Count);
            Vector2 target = targetPositions[index];

            // Pop from list
            targetPositions.RemoveAt(index);

            // Start the MoveTo Coroutine
            StartCoroutine(child.GetComponent<InputLetter>().MoveTo(start, target, scrambleSpeed));
        }
    }

    // Called when a letter is clicked
    public void BeginInput(){
        inputStarted = true;
    }

    public void EndInput(){
        foreach(GameObject letterObj in inputSequence){
            // TODO: This is potentially slow, consider putting the InputLetters in the
            // inputSequence list instead
            letterObj.GetComponent<InputLetter>().DeselectLetter();
        }
        inputStarted = false;
        inputSequence.Clear();
        uiLineRenderer.Points = new Vector2[1];
    }

    // Called when a letter is hovered over and additionally for the first letter
    public void LetterSelected(InputLetter letter){
        GameObject letterGameObject = letter.gameObject;
        if(inputStarted){
            if(!inputSequence.Contains(letterGameObject)){
                inputSequence.Add(letterGameObject);
                letter.SelectLetter();
                Vector3 newPoint = transform.InverseTransformPoint(inputSequence[inputSequence.Count - 1].transform.position);
                List<Vector2> points = new List<Vector2>(uiLineRenderer.Points);
                // Insert 1 before the last point because the last point is handled by the cursor
                // uiLineRenderer.Points should always have at least one point because it freaks out
                // if you have 0, so even if this is the first point we should be safe to insert at
                // points.Count - 1
                points.Insert(points.Count - 1, newPoint);
                uiLineRenderer.Points = points.ToArray();
            }
        }
    }

    void SubmitWord(){
        levelView.SubmitWord();
        EndInput();
    }

    void Update(){
        if(inputStarted){
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint = transform.InverseTransformPoint(mousePoint);

            uiLineRenderer.Points[uiLineRenderer.Points.Length - 1] = mousePoint;
            uiLineRenderer.SetAllDirty(); // We have to set dirty because setting at an index doesn't do it for us
            if(Input.GetMouseButtonUp(0)){
                SubmitWord();
            }
        }
    }

    void OnValidate(){
        PositionLetters();
    }
}
