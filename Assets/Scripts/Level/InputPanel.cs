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
    public GameObject letterPrefab;
    ReadOnlyCollection<char> letters; // Read only because Constaint should handle mutation
    bool inputStarted = false; // Wether we are currently inputting a sequence of letters
    public List<GameObject> inputSequence = new List<GameObject>();
    public Transform inputLetters;
    UILineRenderer uiLineRenderer;
    LevelView levelView;

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
        float radius = GetComponent<RectTransform>().rect.width / 2 - imagePadding;
        // We need to count only the active children because
        // when initalizing another level the old children won't be destroyed yet
        // so we use the activeness to decided whether they're new or old
        int activeChildCount = 0;
        foreach(Transform child in inputLetters){
            if (child.gameObject.activeInHierarchy){
                activeChildCount++;
            }
        }

        float theta = 0;
        float thetaDelta = 2 * Mathf.PI / activeChildCount;
        foreach(Transform childTransform in inputLetters){
            if (childTransform.gameObject.activeInHierarchy){
                childTransform.localPosition = Vector2.zero;
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                childTransform.localPosition = new Vector2(x, y);
                theta += thetaDelta;
            }
        }
    }

    // When called the letters in the array have already been scrambled
    // so we just have to move them to the right place
    public void Scramble(){
        // TODO: Consider catching the possible exception
        for(int i = 0; i < transform.childCount; i++){
            string letter = letters[i].ToString();
            Transform child = FindChildWithLetter(i, letter);
            child.SetSiblingIndex(i);
        }
        PositionLetters();
    }

    Transform FindChildWithLetter(int start, string letter){
        for(int i = start; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);

            Text text = child.GetComponent<Text>();
            if(text.text == letter){
                return child;
            }
        }
        throw new System.Exception(string.Format("Couldn't find child with letter {0}", letter));
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
