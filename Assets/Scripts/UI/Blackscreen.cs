using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackscreen : MonoBehaviour
{
    private bool fade_in;

    private bool fade_out;

    private CanvasGroup canvas_group;

    private void Start()
    {
        canvas_group = GetComponent<CanvasGroup>();

        if(canvas_group == null)
            canvas_group = gameObject.AddComponent<CanvasGroup>();

        FadeOut();
    }

    void Update()
    {
        if (fade_out)
        {
            canvas_group.alpha = canvas_group.alpha - Time.deltaTime * 0.3f;

            if (canvas_group.alpha <= 0)
                gameObject.SetActive(false);
        }

        if (fade_in)
        {
            if(canvas_group.alpha < 1)
            canvas_group.alpha = canvas_group.alpha + Time.deltaTime * 0.3f;
        }
    }

    public void FadeOut() { canvas_group.alpha = 1; fade_out = true; fade_in = false; }

    public void FadeIn() { canvas_group.alpha = 0; fade_in = true; fade_out = false; }

}
