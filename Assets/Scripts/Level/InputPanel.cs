using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using System.Linq;
public class InputPanel : MonoBehaviour
{
    [Range(0, 100)]
    public float radius;
    public GameObject letterPrefab;
    ReadOnlyCollection<char> letters; // Read only because Constaint should handle mutation
    bool inputStarted = false; // Wether we are currently inputting a sequence of letters
    public List<GameObject> inputSequence = new List<GameObject>();

    public LineManager lineManager;
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
        // Delete all children
        // If this is called after the input panel has been initalized
        // than we want to get rid of the old characters
        foreach (Transform child in transform){
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
        this.letters = letters.AsReadOnly();
        foreach (char character in letters){
            GameObject letter = Instantiate(letterPrefab, transform);
            letter.GetComponent<Text>().text = character.ToString();
        }
        PositionLetters();
    }

    void PositionLetters(){
        // We need to count only the active children because
        // when initalizing another level the old children won't be destroyed yet
        // so we use the activeness to decided whether they're new or old
        int activeChildCount = 0;
        foreach(Transform child in transform){
            if (child.gameObject.activeInHierarchy){
                activeChildCount++;
            }
        }
        float theta = 0;
        float thetaDelta = 2 * Mathf.PI / activeChildCount;
        foreach(Transform childTransform in transform){
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
        inputStarted = false;
        lineManager.ClearAllLines();
    }

    // Called when a letter is hovered over and additionally for the first letter
    public void LetterSelected(GameObject letterObj){
        if(inputStarted){
            if(!inputSequence.Contains(letterObj)){
                inputSequence.Add(letterObj);
                if(inputSequence.Count > 1){
                    Vector3 previous = inputSequence[inputSequence.Count - 2].transform.position;
                    Vector3 current = inputSequence[inputSequence.Count - 1].transform.position;
                    lineManager.AddStaticLine(previous, current);
                }
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
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint.z = 90;
            Vector3 previous = inputSequence[inputSequence.Count - 1].transform.position;
            lineManager.UpdateDynamicLine(previous, mousePoint);
            if(Input.GetMouseButtonUp(0)){
                SubmitWord();
            }
        }
    }

    void OnValidate(){
        PositionLetters();
    }
}
