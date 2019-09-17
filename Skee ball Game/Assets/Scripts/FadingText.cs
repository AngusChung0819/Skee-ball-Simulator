using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour
{

    // Initial Variables
    public Text text;
    public float fadeTime = 1.0f;
    public Color textColor;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // OnEnable is called once when object is enabled.
    void OnEnable() {
        StartCoroutine(FadeInAndOut(text));
    }

    // Text Fade in and out
    private IEnumerator FadeInAndOut(Text text) {

        float rate = 1.0f / fadeTime;

        Color startColor = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color endColor = new Color(textColor.r, textColor.g, textColor.b, 0);

        for (int i = 0; i <= 1; i++) {
            float progress = 0.0f;
            while (progress < 1.0f) {
                text.color = Color.Lerp(startColor, endColor, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }
            if (i == 1) {
                text.color = new Color(textColor.r, textColor.g, textColor.b, 1);
            }
            yield return new WaitForSeconds(0.5f);
            startColor = new Color(textColor.r, textColor.g, textColor.b, 0);
            endColor = new Color(textColor.r, textColor.g, textColor.b, 1);
        }

        // Looping itself
        StartCoroutine(FadeInAndOut(text));
    }

}

