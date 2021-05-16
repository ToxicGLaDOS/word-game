using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    public static LevelData levelData = Resources.Load<LevelData>("LevelData/Mercury/0");

    // The path to the dictionary relative from the resources folder (no extention)
    public static string dictionaryPath = "words";
}
