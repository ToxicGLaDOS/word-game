using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordPanelHandler : MonoBehaviour
{
    public GameObject wordPrefab, letterPrefab;

    public Dictionary<string, WordHandler> wordMap = new Dictionary<string, WordHandler>();

    public void InitalizeWords(List<string> words){
        Transform wordsTransform = transform.Find("Words");
        // Make sure the word panel is empty so it can be inialized properly
        foreach(Transform child in wordsTransform){
            Destroy(child.gameObject);
        }
        wordMap.Clear();
        foreach(string word in words){
            GameObject wordObj = Instantiate(wordPrefab, wordsTransform);
            wordObj.name = word;
            wordMap.Add(word, wordObj.GetComponent<WordHandler>());
            foreach(char letter in word){
                GameObject letterObj = Instantiate(letterPrefab, wordObj.transform);
                letterObj.GetComponentInChildren<Text>().text = letter.ToString();
            }
            wordObj.GetComponent<WordHandler>().Initalize();
        }
        List<Transform> children = new List<Transform>();
        foreach(Transform child in wordsTransform){
            children.Add(child);
        }
        SiblingNameLengthComparer comparer = new SiblingNameLengthComparer();
        children.Sort(comparer);
        for(int i = 0; i < children.Count; i++){
            children[i].SetSiblingIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(wordsTransform.GetComponent<RectTransform>()); // Fixes weirdness where adding words doesn't update the layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>()); // Fixes weirdness where adding words doesn't update the layout
    }

    public void RevealWord(string word){
        wordMap[word].Reveal();
    }
}

public class SiblingNameLengthComparer : IComparer<Transform> {
    public int Compare(Transform first, Transform second){
        if(first.name.Length > second.name.Length){
            return 1;
        }
        else if(first.name.Length == second.name.Length){
            return 0;
        }
        else{
            return -1;
        }
    }
}