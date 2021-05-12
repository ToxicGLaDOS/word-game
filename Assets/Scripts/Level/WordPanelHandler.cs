using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class WordPanelHandler : MonoBehaviour
{
    public GameObject columnPrefab, wordPrefab, letterPrefab;

    public Dictionary<string, WordHandler> wordMap = new Dictionary<string, WordHandler>();

    public int maxColumnSize;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitalizeWords(List<string> words){
        // Make sure the word panel is empty so it can be inialized properly
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
        wordMap.Clear();
        int numColumns = (int)Mathf.Ceil(words.Count / (float)maxColumnSize); // Integer division
        for(int i = 0; i < numColumns; i++){
            GameObject columnObj = Instantiate(columnPrefab, transform);
            List<string> columnWords = new List<string>(words.Skip(i*maxColumnSize).Take(maxColumnSize));
            foreach(string word in columnWords){
                GameObject wordObj = Instantiate(wordPrefab, columnObj.transform);
                wordObj.name = word;
                wordMap.Add(word, wordObj.GetComponent<WordHandler>());
                foreach(char letter in word){
                    GameObject letterObj = Instantiate(letterPrefab, wordObj.transform);
                    letterObj.GetComponentInChildren<Text>().text = letter.ToString();
                }
                wordObj.GetComponent<WordHandler>().Initalize();
            }
        }
    }

    public void RevealWord(string word){
        wordMap[word].Reveal();
    }
}
