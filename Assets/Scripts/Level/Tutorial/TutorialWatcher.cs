using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWatcher : MonoBehaviour
{
    static string FirstPlayKey = "Tutorial0";
    public void FirstPlay(){
        if(PlayerPrefs.GetInt(FirstPlayKey, 0) == 0){
            Transform tutorialIntro = transform.Find("TutorialIntro");
            tutorialIntro.gameObject.SetActive(true);
            PlayerPrefs.SetInt(FirstPlayKey, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }
}
