using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPanelLayout : MonoBehaviour
{
    [Range(0, 500)]
    // This is the amount of padding the image has between
    // the true edge of the image and where the letters should be placed
    public float imagePadding;
    [Range(0, 1)]
    public float scrambleSpeed;
    public GameObject letterPrefab;
    public Transform inputLetters;
    private float Radius{
        get {
            return GetComponent<RectTransform>().rect.width / 2 - imagePadding;
        }
    }
    
    public void Initalize(List<char> letters){
        // Delete all children
        // If this is called after the input panel has been initalized
        // than we want to get rid of the old characters
        foreach (Transform child in inputLetters){
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }
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
                int activeChildCount = NumActiveChildren();
                Vector2 position = GetPosition(childIndex);
                childTransform.localPosition = position;
                childIndex++;
            }
        }
    }

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
    void OnValidate(){
        PositionLetters();
    }
}
