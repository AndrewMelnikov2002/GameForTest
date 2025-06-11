using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFadeOut : MonoBehaviour
{
    private bool fade_started = false;

    private CanvasGroup canvas_group;

    private void Start()
    {
        canvas_group = GetComponent <CanvasGroup> ();
    }

    private void Update()
    {
        if (canvas_group == null)
            return;

        if (fade_started)
            canvas_group.alpha = canvas_group.alpha -= Time.deltaTime;

        if (canvas_group.alpha <= 0)
            Destroy(gameObject);
    }

    public void StartFadeOut() { fade_started = true; }
}
