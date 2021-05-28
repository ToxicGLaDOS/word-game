using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWatcher : MonoBehaviour
{
    public GameObject tutorialScreen;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Tutorial_Intro", 0) == 0){
            tutorialScreen.SetActive(true);
            PlayerPrefs.SetInt("Tutorial_Intro", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
