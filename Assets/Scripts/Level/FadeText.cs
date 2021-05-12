using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeText : MonoBehaviour
{
    [Range(0, 2)]
    public float fadeSpeed;
    public float alpha = 1;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        alpha = Mathf.Clamp01(alpha - fadeSpeed * Time.deltaTime);
        ApplyAlpha();
    }

    void ApplyAlpha(){
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    public void ResetAlpha(){
        alpha = 1;
        ApplyAlpha();
    }
}
