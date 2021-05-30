using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Script to manage a specific tutorial sequence
public class Tutorial : MonoBehaviour
{
    public UnityEvent onStart;
    // Start is called before the first frame update
    void Start()
    {
        if(onStart != null){
            onStart.Invoke();
        }
    }
}
