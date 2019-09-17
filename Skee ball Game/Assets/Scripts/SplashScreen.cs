using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;  // a low number means it renders on top
    private float alpha = 1.0f;     // the texture's alpha value between 0 and 1
    private int fadeDir = -1;       // the direction to fade; in = 1 ;out =-1


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnGUI() {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction) {
        fadeDir = direction;
        return (fadeSpeed);
    }

    private void OnLevelWasLoaded() {
        // alpha = 1;
        BeginFade(-1);  // call the fade in function
    }
}
