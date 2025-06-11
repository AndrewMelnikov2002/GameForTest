using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bullet_speed = 1f;

    [SerializeField] private float life_time;
    private float timer;

    private void Start()
    {
        timer = life_time;
    }

    void Update()
    {
        transform.position = transform.position + (transform.right * (bullet_speed * Time.deltaTime));

        timer -= Time.deltaTime;

        if (timer <= 0)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Zombie" & !collision.isTrigger)
        {
            collision.GetComponent<ZombieController>().GetDamage(34f, collision.transform.position - transform.position);
            Destroy(gameObject);
        }
    }
}
