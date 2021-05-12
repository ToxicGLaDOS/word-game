using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPresenter : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelSelectMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void SelectLevel(LevelData level){
        GlobalValues.levelData = level;
        SceneManager.LoadScene("Main");
    }

    public void OpenLevelSelectMenu(){
        MainMenu.SetActive(false);
        LevelSelectMenu.SetActive(true);
    }
}
