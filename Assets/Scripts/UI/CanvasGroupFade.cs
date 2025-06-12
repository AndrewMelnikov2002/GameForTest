using UnityEngine;

public class CanvasGroupFade : MonoBehaviour
{
    private CanvasGroup canvas_group;

    private bool fade_in = false;
    private bool fade_out = false;

    private float fade_in_speed = 1;
    private float fade_out_speed = 1;

    private void Awake()
    {
        canvas_group = GetComponent<CanvasGroup>();

        if (canvas_group == null)
            canvas_group = gameObject.AddComponent<CanvasGroup>();
    }


    private void Update()
    {
        if (fade_in)
            FadeInProcces();
        if (fade_out)
            FadeOutProcces();
    }

    private void FadeOutProcces()
    {
        if (fade_in)
            fade_in = false;

        if (canvas_group.alpha > 0)
            canvas_group.alpha -= Time.deltaTime * fade_out_speed;
    }

    private void FadeInProcces()
    {
        if (canvas_group.alpha < 1)
            canvas_group.alpha += Time.deltaTime * fade_in_speed;
    }


    public void StartFadeIn(float speed) 
    {
        fade_in = true;
        fade_out = false;
        fade_in_speed = speed; 
        canvas_group.alpha = 0;
        canvas_group.blocksRaycasts = true;
        canvas_group.interactable = true;
    }

    public void StartFadeOut(float speed) 
    { 
        fade_out = true;
        fade_in = false;
        fade_out_speed = speed;
        canvas_group.alpha = 1;
        canvas_group.blocksRaycasts = false;
        canvas_group.interactable = false;
    }
}

