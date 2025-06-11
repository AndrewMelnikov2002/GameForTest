using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScreen : MonoBehaviour
{
    private CanvasGroup canvas_group;

    private void Awake()
    {
        canvas_group = GetComponent<CanvasGroup>();

        if (canvas_group == null)
            canvas_group = gameObject.AddComponent<CanvasGroup>();
    }

    void Update()
    {
        canvas_group.alpha += Time.deltaTime * 0.5f;
    }

    public void SetAlpha(float alpha) { canvas_group.alpha = alpha; }
}
