using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInputPanel : InputPanel
{
    // Start is called before the first frame update
    public InputSpoof cursor;
    RectTransform cursorRectTransform;
    protected override void Start()
    {
        base.Start();
        cursorRectTransform = cursor.GetComponent<RectTransform>();
    }

    protected override Vector3 GetCursorPosition() {
        return transform.InverseTransformPoint(cursorRectTransform.position);
    }

    // This function essentially makes SubmitWord public for the tutorial version of InputPanel
    public void EndTutorialSequence(){
        SubmitWord();
    }

    protected override void SubmitWord(){
        inputWord.Clear();
        EndInput();
    }

    protected override bool CursorReleased(){
        return false;
    }
}
