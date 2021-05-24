using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        int currentGroup = PlayerPrefs.GetInt("CurrentGroup", 0);
        for(int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            // We haven't unlocked this group yet
            if(i > currentGroup){
                child.gameObject.SetActive(false);
            }
            // We have completed this group
            else if(i < currentGroup){
                // Leave it all active
            }
            // We have unlocked this group
            else{
                Transform levels = child.FindDeepChild("Levels");
                for(int j = 0; j < levels.childCount; j++){
                    if(j > currentLevel){
                        levels.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}

public static class TransformExtension {
    public static Transform FindDeepChild(this Transform parentTransform, string childName){
        Transform correctChild = parentTransform.Find(childName);
        if(correctChild != null){
            return correctChild;
        }
        else{
            foreach(Transform child in parentTransform){
                correctChild = FindDeepChild(child, childName);
                if(correctChild != null){
                    return correctChild;
                }
            }
        }
        return null;
    }
}
