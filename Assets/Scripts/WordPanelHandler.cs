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
        //List<string> words = new List<string>();
        //words.Add("espalier");
        //words.Add("jebel");
        //words.Add("raddlel");
        //words.Add("windrose");
        //words.Add("yabby");
        //words.Add("xerasia");
        //words.Add("polystyle");
        //words.Add("yearling");
        //words.Add("vernition");
        //words.Add("livedo");
        //words.Add("yapp");
        //words.Add("octad");
        //words.Add("vocoid");
        //words.Add("lordosis");
        //words.Add("yogini");
        //words.Add("utricle");
        //words.Add("tuatara");
        //words.Add("yen");
        //words.Add("madid");

        //AddWords(words);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWords(List<string> words){
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
