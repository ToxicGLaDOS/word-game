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

    public void Test(GameObject go){
        Debug.Log(string.Format("Success {0}", go));
    }

    public void Test(LevelData level){
        Debug.Log("Success");
    }

    public void SelectLevel(LevelData level){
        presenter.SelectLevel(level);
    }

    public void OpenLevelSelectMenu(){
        presenter.OpenLevelSelectMenu();
    }
}
