
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private float amount = 0;
    private Transform target;


    [Header("Main")]
    [SerializeField] private float height = 1;

    [Space(20)]
    [SerializeField] private Image bar_fill;
    [SerializeField] private Image white_part;


    private void Update()
    {
        white_part.fillAmount = Mathf.Lerp(white_part.fillAmount, amount-0.01f, Time.deltaTime * 2f);

        bar_fill.fillAmount = amount;

        if(target!= null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position + (Vector3.up * height));
        }
    }

    public void SetAmount(float amount) { this.amount = amount; }

    public void SetTarget(Transform target) { this.target = target; }
}
