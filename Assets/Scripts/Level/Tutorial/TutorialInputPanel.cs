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

    public void EndTutorialSequence(){
        SubmitWord();
    }

    protected override void SubmitWord(){
        EndInput();
    }

    protected override bool CursorReleased(){
        print(cursor.MouseButtonUp);
        return cursor.MouseButtonUp;
    }

    public override void Initalize(List<char> letters){
        // Shouldn't be called anyway, but for safety we disable it
    }
}
