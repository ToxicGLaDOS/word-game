using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{
    [Range(0, 100)]
    public float radius;
    public GameObject letterPrefab;
    List<char> letters;
    bool inputStarted = false; // Wether we are currently inputting a sequence of letters
    public List<GameObject> inputSequence = new List<GameObject>();

    LevelView levelView;

    public string InputWord{
        get {
            string s = "";
            foreach(GameObject letterObj in inputSequence){
                s += char.Parse(letterObj.GetComponent<Text>().text);
            }
            return s;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelView = FindObjectOfType<LevelView>();
    }

    public void Initalize(List<char> letters){
        this.letters = letters;
        foreach (char character in letters){
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.GetComponent<Text>().text = character.ToString();
        }
        PositionLetters();
    }

    void PositionLetters(){
        float theta = 0;
        float thetaDelta = 2 * Mathf.PI / transform.childCount;
        foreach(Transform childTransform in transform){
            childTransform.localPosition = Vector2.zero;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            childTransform.localPosition = new Vector2(x, y);
            theta += thetaDelta;
        }
    }

    // Called when a letter is clicked
    public void BeginInput(){
        inputStarted = true;
    }

    public void EndInput(){
        inputStarted = false;
    }

    // Called when a letter is hovered over
    public void LetterSelected(GameObject letterObj){
        if(inputStarted){
            if(!inputSequence.Contains(letterObj)){
                inputSequence.Add(letterObj);
            }
        }
    }

    void SubmitWord(){
        levelView.SubmitWord();
        inputSequence.Clear();
        EndInput();
    }

    void Update(){
        if(inputStarted){
            if(Input.GetMouseButtonUp(0)){
                SubmitWord();
            }
        }
    }

    void OnValidate(){
        PositionLetters();
    }
}
