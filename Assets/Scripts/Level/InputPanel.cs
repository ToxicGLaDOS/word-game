using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.UI.Extensions;
public class InputPanel : MonoBehaviour
{
    
    bool inputStarted = false; // Wether we are currently inputting a sequence of letters
    public List<GameObject> inputSequence = new List<GameObject>();
    public Transform inputLetters;
    public InputWord inputWord;
    UILineRenderer uiLineRenderer;
    LevelView levelView;

    protected bool InputStarted {
        get {
            return inputStarted;
        }
    }

    public string InputWord{
        get {
            string s = "";
            foreach(GameObject letterObj in inputSequence){
                s += letterObj.GetComponent<InputLetter>().Letter;
            }
            return s;
        }
    }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        uiLineRenderer = GetComponentInChildren<UILineRenderer>();
        levelView = FindObjectOfType<LevelView>();
    }

    // Called when a letter is clicked
    public void BeginInput(){
        inputStarted = true;
    }

    protected void EndInput(){
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

                // Prevents the default value of (0,0) from sneaking in for 1 frame
                // under certain (read unknown) circumstances
                SetLineRendererMousePoint(GetCursorPosition());

                inputWord.AddLetter(letter.Letter);
            }
        }
    }

    virtual protected void SubmitWord(){
        levelView.SubmitWord();
        inputWord.Clear();
        EndInput();
    }

    protected void SetLineRendererMousePoint(Vector2 point){
        uiLineRenderer.Points[uiLineRenderer.Points.Length - 1] = point;
        uiLineRenderer.SetAllDirty(); // We have to set dirty because setting at an index doesn't do it for us
    }

    virtual protected Vector3 GetCursorPosition(){
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint = transform.InverseTransformPoint(mousePoint);
        return mousePoint;
    }

    virtual protected bool CursorReleased(){
        return Input.GetMouseButtonUp(0);
    }

    void Update(){
        if(inputStarted){
            Vector3 mousePoint = GetCursorPosition();

            SetLineRendererMousePoint(mousePoint);
            if(CursorReleased()){
                SubmitWord();
            }
        }
    }
}
