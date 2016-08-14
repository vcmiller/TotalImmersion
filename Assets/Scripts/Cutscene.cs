using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour {
    private CanvasGroup[] slides;
    private int slide = 0;

	// Use this for initialization
	void Start () {
        slides = GetComponentsInChildren<CanvasGroup>();
        foreach (CanvasGroup slide in slides) {
            slide.alpha = 0;
        }

        slides[0].alpha = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown) {
            if (slide < slides.Length - 1) {
                slides[slide].alpha = 0;
                slides[++slide].alpha = 1;
            } else {
                Application.LoadLevel("Level 1");
            }
        }
	}
}
