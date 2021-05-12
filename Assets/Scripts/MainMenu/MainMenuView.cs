using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    MainMenuPresenter presenter;
    // Start is called before the first frame update
    void Start()
    {
        presenter = GetComponent<MainMenuPresenter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel(LevelData level){
        presenter.SelectLevel(level);
    }

    public void OpenLevelSelectMenu(){
        presenter.OpenLevelSelectMenu();
    }
}
