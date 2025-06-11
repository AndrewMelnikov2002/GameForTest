using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private float tack_speed;

    [SerializeField] private Transform target;

    private void Update()
    {
        if(target != null)
        {
            Vector3 final_positon = target.position;

            final_positon.z = -10;

            transform.position = Vector3.Lerp(transform.position, final_positon, tack_speed * Time.deltaTime); 
        }
    }
    
    public void SetTarget(Transform target) { this.target = target; }
}
